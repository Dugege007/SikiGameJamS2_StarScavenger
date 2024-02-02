using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Player : ViewController
    {
        public static Player Default;

        private float mCurrentCTime = 0;

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            SelfRigidbody2D.velocity = Vector3.up * Global.MoveSpeed.Value;

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
                            //TODO ����������Ч
                            // ��������
                            this.DestroyGameObjGracefully();

                            // ��Ϸ����
                            UIKit.OpenPanel<GameOverPanel>();
                            // �л�����
                        }
                        else
                        {
                            //TODO ����������Ч

                        }
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            mCurrentCTime += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal > 0)
            {
                //TODO ��ת����
            }
            else if (horizontal < 0)
            {
                //TODO ��ת����
            }

            // ת��
            if (horizontal != 0)
            {
                transform.Rotate(0, 0, -horizontal * Global.RotateSpeed.Value);
                // ����ȼ��
                ConsumptionFuel();
            }

            // �ƶ�
            if (vertical > 0)
            {
                // ����
                Global.MoveSpeed.Value += Time.fixedDeltaTime * Global.Acceleration.Value;

                // ����ȼ��
                ConsumptionFuel();
            }

            SelfRigidbody2D.velocity = transform.up * Global.MoveSpeed.Value;
        }

        private void ConsumptionFuel()
        {
            if (mCurrentCTime >= 0.1f)
            {
                mCurrentCTime = 0;
                Global.Fuel.Value -= Global.FuelConsumptionSpeed.Value;
            }
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
