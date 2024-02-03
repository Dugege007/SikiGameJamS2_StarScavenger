using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Player : ViewController
    {
        public static Player Default;

        public bool CanAttack = true;

        private float mFCTime = 0;
        private float mAutoFCTime = 0;
        private float mReduceHPTime = 0;

        private float mCurrentMoveSpeed;
        private bool mIsTurning = false;

        private Vector2 mGravity = Vector2.zero;
        private Vector2 mPropulsiveForce = Vector2.zero;

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            LineRenderer1.positionCount = Global.PathResolution.Value;
            LineRenderer2.positionCount = Global.PathResolution.Value;
            Projectile.Hide();
            LineRenderer1.Hide();
            LineRenderer2.Hide();

            SelfRigidbody2D.velocity = Vector3.up * Global.CurrentSpeed.Value;

            // 注册碰撞事件
            HurtBox.OnCollisionEnter2DEvent(collider2D =>
            {
                HitHurtBox hitBox = collider2D.gameObject.GetComponentInChildren<HitHurtBox>();

                if (hitBox != null)
                {
                    if (hitBox.Owner.CompareTag("Asteroid"))
                    {
                        Global.HP.Value--;
                    }
                    if (hitBox.Owner.CompareTag("Planet"))
                    {
                        Global.HP.Value = 0;
                    }

                    Debug.Log("CurrentHP: " + Global.HP.Value);
                    //TODO 播放碰撞音效
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.IsReducingHP.RegisterWithInitValue(isReducingHP =>
            {
                mReduceHPTime = 0;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            mFCTime += Time.deltaTime;
            mAutoFCTime += Time.deltaTime;

            if (Global.HP.Value <= 0)
            {
                //TODO 爆炸特效
                //TODO 失败音效
                gameObject.DestroySelfGracefully();
                UIKit.OpenPanel<GameOverPanel>();
                return;
            }

            // 持续扣血状态
            if (Global.IsReducingHP.Value)
            {
                mReduceHPTime += Time.deltaTime;
                if (mReduceHPTime > 5f)
                {
                    Global.HP.Value--;
                    mReduceHPTime = 0;
                }
            }

            // 燃料为 0 时
            if (Global.Fuel.Value < 1)
            {
                if (Global.IsReducingHP.Value == false)
                    Global.IsReducingHP.Value = true;
                return;
            }
            else
            {
                if (Global.IsReducingHP.Value)
                    Global.IsReducingHP.Value = false;
            }

            // 常时耗费燃料
            if (mAutoFCTime > Global.FuelConsumptTime.Value)
            {
                Global.Fuel.Value--;
                mAutoFCTime = 0;
            }

            if (CanAttack)
            {
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
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (Global.Fuel.Value > 0)
            {
                // 获取速度大小
                Global.CurrentSpeed.Value = SelfRigidbody2D.velocity.magnitude;

                // 转向
                if (horizontal != 0)
                {
                    mIsTurning = true;
                    transform.Rotate(0, 0, -horizontal * Global.RotateSpeed.Value);
                    // 调整速度方向
                    SelfRigidbody2D.velocity = transform.up * Global.CurrentSpeed.Value;
                    // 消耗燃料
                    FuelConsumpt(0.5f, Global.FuelConsumpt.Value);
                }
                else
                {
                    mIsTurning = false;
                }
                //TODO 转向动画

                // 移动
                if (vertical > 0)
                {
                    mPropulsiveForce = transform.up * Global.PropulsiveForceValue.Value;
                    FuelConsumpt(0.1f, Global.FuelConsumpt.Value);
                }
                else if (vertical < 0)
                {
                    mPropulsiveForce = -transform.up * Global.PropulsiveForceValue.Value;
                    FuelConsumpt(0.1f, Global.FuelConsumpt.Value);
                }
                else
                {
                    mPropulsiveForce = Vector2.zero;
                }
            }
            else
            {
                Global.Fuel.Value = 0;
            }

            Vector2 resulForces = mPropulsiveForce + mGravity;
            SelfRigidbody2D.AddForce(resulForces, ForceMode2D.Force);
        }

        /// <summary>
        /// 消耗燃料
        /// </summary>
        /// <param name="cTime">间隔时间</param>
        /// <param name="consumptionValue">消耗数量</param>
        private void FuelConsumpt(float cTime, int consumptionValue = 1)
        {
            if (mFCTime >= cTime)
            {
                mFCTime = 0;
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
                UpdatePathWithRK4(planet, transform.position, SelfRigidbody2D.velocity);

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

        private void UpdatePathWithRK4(Planet planet, Vector2 currentPos, Vector2 currentVelocity)
        {
            float deltaTime = Global.PathPredictTime.Value / Global.PathResolution.Value;

            for (int i = 0; i < Global.PathResolution.Value; i++)
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

                if (Vector2.Distance(planet.transform.position, finalPos) < planet.Radius)
                {
                    // 使用计算的位置更新 LineRenderer
                    LineRenderer1.SetPosition(i, finalPos);
                    if (Global.IsAboutCollide.Value == false)
                        Global.IsAboutCollide.Value = true;
                    continue;
                }
                else
                {
                    LineRenderer1.SetPosition(i, finalPos);
                    if (Global.IsAboutCollide.Value)
                        Global.IsAboutCollide.Value = false;
                    // 为下一个迭代准备当前速度和位置
                    currentVelocity = finalVelocity;
                    currentPos = finalPos;
                }
            }
        }

        private Vector2 NextPos(Vector2 currentPos, Vector2 currentVelocity, Planet planet)
        {
            float deltaTime = Global.PathPredictTime.Value / Global.PathResolution.Value;
            Vector2 acceleration = CalculateAcceleration(currentPos, planet);
            // 使用简化的方法来获取第一个预测点的位置，基于当前速度和加速度
            Vector2 firstPredictedPos = currentPos + currentVelocity * deltaTime + 0.5f * acceleration * Mathf.Pow(deltaTime, 2);
            return firstPredictedPos;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
