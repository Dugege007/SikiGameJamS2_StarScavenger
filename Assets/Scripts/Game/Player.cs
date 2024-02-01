using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Player : ViewController
    {
        public static Player Default;

        public float MoveSpeed = 1f;
        public float Acceleration = 0.5f;
        public float RotateSpeed = 0.5f;

        private float mCurrentCTime = 0;

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            SelfRigidbody2D.velocity = Vector3.up * MoveSpeed;

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

            // 转向
            if (horizontal != 0)
            {
                transform.Rotate(0, 0, -horizontal * RotateSpeed);
                // 消耗燃料
                ConsumptionFuel();
            }

            // 移动
            if (vertical > 0)
            {
                // 加速
                MoveSpeed += Time.fixedDeltaTime * Acceleration;

                // 消耗燃料
                ConsumptionFuel();
            }

            SelfRigidbody2D.velocity = transform.up * MoveSpeed;
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
