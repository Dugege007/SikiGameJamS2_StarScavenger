using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class PlayerSpaceShip : ViewController
    {
        public static PlayerSpaceShip Default;

        public float MoveSpeed = 1f;
        public float Acceleration = 0.5f;
        public float RotateSpeed = 0.2f;

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            SelfRigidbody2D.velocity = Vector3.up * MoveSpeed;
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
                transform.Rotate(0, 0, -horizontal * RotateSpeed);

            // 移动
            if (vertical > 0)
            {
                MoveSpeed += Time.fixedDeltaTime * Acceleration;
                Debug.Log("当前速度：" + MoveSpeed);

                //TODO 消耗燃料
            }

            SelfRigidbody2D.velocity = transform.up * MoveSpeed;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
