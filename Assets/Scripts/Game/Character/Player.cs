using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Player : ViewController
    {
        public static Player Default;

        private float mCurrentCTime = 0;
        private float mCurrentMoveSpeed;
        private bool mIsTurning = false;

        private Vector2 mGravity = Vector2.zero;
        private Vector2 mPropulsiveForce = Vector2.zero;
        //private Vector2 mHorizontalForce = Vector2.zero;

        public int pathResolution = 50; // 路径解析度，即路径上的点数
        public float pathPredictTime = 5f; // 预测路径的时间长度

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            LineRenderer1.positionCount = pathResolution;
            LineRenderer2.positionCount = pathResolution;
            Projectile.Hide();

            mCurrentMoveSpeed = Global.MoveSpeed.Value;
            SelfRigidbody2D.velocity = Vector3.up * mCurrentMoveSpeed;

            HurtBox.OnCollisionEnter2DEvent(collider2D =>
            {
                HitHurtBox hitBox = collider2D.gameObject.GetComponentInChildren<HitHurtBox>();

                if (hitBox != null)
                {
                    if (hitBox.Owner.CompareTag("Asteroid"))
                    {
                        Global.HP.Value--;
                        if (Global.HP.Value <= 0)
                        {
                            //TODO 播放死亡音效
                            // 销毁自身
                            this.DestroyGameObjGracefully();

                            // 游戏结束
                            UIKit.OpenPanel<GameOverPanel>();
                            // 切换音乐
                        }
                        else
                        {
                            //TODO 播放受伤音效

                        }
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            mCurrentCTime += Time.deltaTime;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Global.Fuel.Value--;

                Projectile.Instantiate()
                    .Position(transform.position + transform.up * 0.5f)
                    .Self(self =>
                    {
                        Vector2 dir = (mousePos - self.transform.position).normalized;
                        self.gameObject.transform.up = dir;
                        self.GetComponent<ProjectileController>().Owner = this.gameObject;
                    })
                    .Show();
            }
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal > 0)
            {
                //TODO 右转动画
            }
            else if (horizontal < 0)
            {
                //TODO 左转动画
            }

            // 获取速度大小
            mCurrentMoveSpeed = SelfRigidbody2D.velocity.magnitude;

            // 转向
            if (horizontal != 0)
            {
                mIsTurning = true;
                transform.Rotate(0, 0, -horizontal * Global.RotateSpeed.Value);
                // 调整速度方向
                SelfRigidbody2D.velocity = transform.up * mCurrentMoveSpeed;
                // 消耗燃料
                ConsumptionFuel(0.5f, Global.FuelConsumption.Value);
            }
            else
            {
                mIsTurning = false;
            }

            // 移动
            if (vertical > 0)
            {
                mPropulsiveForce = transform.up * Global.PropulsiveForceValue.Value;
                // 消耗燃料
                ConsumptionFuel(0.1f, Global.FuelConsumption.Value);
            }
            else if (vertical < 0)
            {
                mPropulsiveForce = -transform.up * Global.PropulsiveForceValue.Value;
                // 消耗燃料
                ConsumptionFuel(0.1f, Global.FuelConsumption.Value);
            }
            else
            {
                mPropulsiveForce = Vector2.zero;
            }

            Vector2 resulForces = mPropulsiveForce + mGravity;
            SelfRigidbody2D.AddForce(resulForces, ForceMode2D.Force);
        }

        /// <summary>
        /// 消耗燃料
        /// </summary>
        /// <param name="cTime">间隔时间</param>
        /// <param name="consumptionValue">消耗数量</param>
        private void ConsumptionFuel(float cTime, int consumptionValue = 1)
        {
            if (mCurrentCTime >= cTime)
            {
                mCurrentCTime = 0;
                Global.Fuel.Value -= consumptionValue;
            }
        }

        public void GravityEffect(Planet planet)
        {
            if (planet != null)
            {
                // 计算重力方向：从玩家指向行星的向量
                Vector2 gravityDirection = (Vector2)planet.transform.position - SelfRigidbody2D.position;
                // 计算重力加速度：使用万有引力公式
                mGravity = gravityDirection.normalized * planet.Gravity / Mathf.Pow(gravityDirection.magnitude, 2);
                // 施加重力力量到玩家上
                SelfRigidbody2D.AddForce(mGravity * SelfRigidbody2D.mass);
                // 如果当前没有在转向
                if (mIsTurning == false)
                {
                    // 计算第一个预测点的位置来调整朝向
                    Vector2 firstPredictedPosition = NextPos(transform.position, SelfRigidbody2D.velocity, planet);
                    Vector2 directionToLook = firstPredictedPosition - (Vector2)transform.position;
                    float angle = Mathf.Atan2(directionToLook.y, directionToLook.x) * Mathf.Rad2Deg - 90;
                    transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, angle, 1f));
                }

                // 使用RK4方法更新预测路径
                UpdatePathRK4(planet, transform.position, SelfRigidbody2D.velocity);

                //Debug.Log("重力方向：" + gravityDirection.normalized + "\n" + "重力大小：" + mGravity);
            }
            else
            {
                mGravity = Vector2.zero;
            }
        }

        private Vector2 CalculateAcceleration(Vector2 position, Planet planet)
        {
            // 到星球中心的距离
            Vector2 gravityDirection = (Vector2)planet.transform.position - position;
            // 星球表面重力加速度
            float gravityAtSurface = planet.Gravity;
            // 星球半径的平方
            float planetRadiusSquared = Mathf.Pow(planet.Radius, 2);

            // 计算加速度
            Vector2 acceleration = gravityDirection.normalized * (gravityAtSurface * planetRadiusSquared / gravityDirection.sqrMagnitude);

            return acceleration;
        }

        private void UpdatePathRK4(Planet planet, Vector2 currentPos, Vector2 currentVelocity)
        {
            float deltaTime = pathPredictTime / pathResolution;

            for (int i = 0; i < pathResolution; i++)
            {
                // RK4方法的四个步骤
                Vector2 k1_vel = currentVelocity;
                Vector2 k1_acc = CalculateAcceleration(currentPos, planet);

                Vector2 k2_vel = currentVelocity + k1_acc * (deltaTime / 2f);
                Vector2 k2_acc = CalculateAcceleration(currentPos + k1_vel * (deltaTime / 2f), planet);

                Vector2 k3_vel = currentVelocity + k2_acc * (deltaTime / 2f);
                Vector2 k3_acc = CalculateAcceleration(currentPos + k2_vel * (deltaTime / 2f), planet);

                Vector2 k4_vel = currentVelocity + k3_acc * deltaTime;
                Vector2 k4_acc = CalculateAcceleration(currentPos + k3_vel * deltaTime, planet);

                // 使用四个斜率的加权平均值来更新速度和位置
                Vector2 finalVelocity = currentVelocity + (k1_acc + 2f * (k2_acc + k3_acc) + k4_acc) * (deltaTime / 6f);
                Vector2 finalPos = currentPos + (k1_vel + 2f * (k2_vel + k3_vel) + k4_vel) * (deltaTime / 6f);

                // 使用计算的位置更新 LineRenderer
                LineRenderer1.SetPosition(i, finalPos);

                // 为下一个迭代准备当前速度和位置
                currentVelocity = finalVelocity;
                currentPos = finalPos;
            }
        }

        private Vector2 NextPos(Vector2 currentPos, Vector2 currentVelocity, Planet planet)
        {
            float deltaTime = pathPredictTime / pathResolution;
            Vector2 acceleration = CalculateAcceleration(currentPos, planet);
            // 使用简化的方法来获取第一个预测点的位置，基于当前速度和加速度
            Vector2 firstPredictedPos = currentPos + currentVelocity * deltaTime + 0.5f * acceleration * Mathf.Pow(deltaTime, 2);
            return firstPredictedPos;
        }

        private void UpdatePath(Planet planet, Vector2 currentPos, Vector2 currentVelocity, Vector2 currentAcceleration)
        {
            Vector2 predictedPos;
            float t = pathPredictTime / pathResolution; // 每一步的时间间隔

            // 设置LineRenderer的点
            for (int i = 0; i < pathResolution; i++)
            {
                // 更新位置：S = S0 + U*t + 0.5*a*t^2
                predictedPos = currentPos + currentVelocity * t + 0.5f * Mathf.Pow(t, 2) * currentAcceleration;

                // 将计算的位置设置为轨迹的一部分
                LineRenderer2.SetPosition(i, predictedPos);

                // 基于新的预测位置，计算下一点的重力加速度
                Vector2 gravityDirection = (Vector2)planet.transform.position - predictedPos;
                currentAcceleration = gravityDirection.normalized * planet.Gravity / Mathf.Pow(gravityDirection.magnitude, 2);

                // 为下一次迭代更新当前位置
                currentPos = predictedPos;

                // 更新速度：V = U + a*t
                currentVelocity += currentAcceleration * t;
            }
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
