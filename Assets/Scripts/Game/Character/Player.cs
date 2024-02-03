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

            // ע����ײ�¼�
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
                    //TODO ������ײ��Ч
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
                //TODO ��ը��Ч
                //TODO ʧ����Ч
                gameObject.DestroySelfGracefully();
                UIKit.OpenPanel<GameOverPanel>();
                return;
            }

            // ������Ѫ״̬
            if (Global.IsReducingHP.Value)
            {
                mReduceHPTime += Time.deltaTime;
                if (mReduceHPTime > 5f)
                {
                    Global.HP.Value--;
                    mReduceHPTime = 0;
                }
            }

            // ȼ��Ϊ 0 ʱ
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

            // ��ʱ�ķ�ȼ��
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
                // ��ȡ�ٶȴ�С
                Global.CurrentSpeed.Value = SelfRigidbody2D.velocity.magnitude;

                // ת��
                if (horizontal != 0)
                {
                    mIsTurning = true;
                    transform.Rotate(0, 0, -horizontal * Global.RotateSpeed.Value);
                    // �����ٶȷ���
                    SelfRigidbody2D.velocity = transform.up * Global.CurrentSpeed.Value;
                    // ����ȼ��
                    FuelConsumpt(0.5f, Global.FuelConsumpt.Value);
                }
                else
                {
                    mIsTurning = false;
                }
                //TODO ת�򶯻�

                // �ƶ�
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
        /// ����ȼ��
        /// </summary>
        /// <param name="cTime">���ʱ��</param>
        /// <param name="consumptionValue">��������</param>
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
                // �����������򣺴����ָ�����ǵ�����
                Vector2 gravityDirection = (Vector2)planet.transform.position - SelfRigidbody2D.position;
                // �����������ٶȣ�ʹ������������ʽ
                mGravity = gravityDirection.normalized * planet.Gravity / Mathf.Pow(gravityDirection.magnitude, 2);
                // ʩ�����������������
                SelfRigidbody2D.AddForce(mGravity * SelfRigidbody2D.mass);
                // �����ǰû����ת��
                if (mIsTurning == false)
                {
                    // �����һ��Ԥ����λ������������
                    Vector2 firstPredictedPosition = NextPos(transform.position, SelfRigidbody2D.velocity, planet);
                    Vector2 directionToLook = firstPredictedPosition - (Vector2)transform.position;
                    float angle = Mathf.Atan2(directionToLook.y, directionToLook.x) * Mathf.Rad2Deg - 90;
                    transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, angle, 1f));
                }

                // ʹ��RK4��������Ԥ��·��
                UpdatePathWithRK4(planet, transform.position, SelfRigidbody2D.velocity);

                //Debug.Log("��������" + gravityDirection.normalized + "\n" + "������С��" + mGravity);
            }
            else
            {
                mGravity = Vector2.zero;
            }
        }

        private Vector2 CalculateAcceleration(Vector2 position, Planet planet)
        {
            // ���������ĵľ���
            Vector2 gravityDirection = (Vector2)planet.transform.position - position;
            // ��������������ٶ�
            float gravityAtSurface = planet.Gravity;
            // ����뾶��ƽ��
            float planetRadiusSquared = Mathf.Pow(planet.Radius, 2);

            // ������ٶ�
            Vector2 acceleration = gravityDirection.normalized * (gravityAtSurface * planetRadiusSquared / gravityDirection.sqrMagnitude);

            return acceleration;
        }

        private void UpdatePathWithRK4(Planet planet, Vector2 currentPos, Vector2 currentVelocity)
        {
            float deltaTime = Global.PathPredictTime.Value / Global.PathResolution.Value;

            for (int i = 0; i < Global.PathResolution.Value; i++)
            {
                // RK4�������ĸ�����
                Vector2 k1_vel = currentVelocity;
                Vector2 k1_acc = CalculateAcceleration(currentPos, planet);

                Vector2 k2_vel = currentVelocity + k1_acc * (deltaTime / 2f);
                Vector2 k2_acc = CalculateAcceleration(currentPos + k1_vel * (deltaTime / 2f), planet);

                Vector2 k3_vel = currentVelocity + k2_acc * (deltaTime / 2f);
                Vector2 k3_acc = CalculateAcceleration(currentPos + k2_vel * (deltaTime / 2f), planet);

                Vector2 k4_vel = currentVelocity + k3_acc * deltaTime;
                Vector2 k4_acc = CalculateAcceleration(currentPos + k3_vel * deltaTime, planet);

                // ʹ���ĸ�б�ʵļ�Ȩƽ��ֵ�������ٶȺ�λ��
                Vector2 finalVelocity = currentVelocity + (k1_acc + 2f * (k2_acc + k3_acc) + k4_acc) * (deltaTime / 6f);
                Vector2 finalPos = currentPos + (k1_vel + 2f * (k2_vel + k3_vel) + k4_vel) * (deltaTime / 6f);

                if (Vector2.Distance(planet.transform.position, finalPos) < planet.Radius)
                {
                    // ʹ�ü����λ�ø��� LineRenderer
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
                    // Ϊ��һ������׼����ǰ�ٶȺ�λ��
                    currentVelocity = finalVelocity;
                    currentPos = finalPos;
                }
            }
        }

        private Vector2 NextPos(Vector2 currentPos, Vector2 currentVelocity, Planet planet)
        {
            float deltaTime = Global.PathPredictTime.Value / Global.PathResolution.Value;
            Vector2 acceleration = CalculateAcceleration(currentPos, planet);
            // ʹ�ü򻯵ķ�������ȡ��һ��Ԥ����λ�ã����ڵ�ǰ�ٶȺͼ��ٶ�
            Vector2 firstPredictedPos = currentPos + currentVelocity * deltaTime + 0.5f * acceleration * Mathf.Pow(deltaTime, 2);
            return firstPredictedPos;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
