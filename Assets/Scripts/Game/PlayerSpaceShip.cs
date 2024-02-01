using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class PlayerSpaceShip : ViewController
    {
        public float MoveSpeed = 1f;
        public float Acceleration = 0.5f;
        public float RotateSpeed = 0.1f;

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
                //TODO ��ת����
            }
            else if (horizontal < 0)
            {
                //TODO ��ת����
            }

            // ת��
            if (horizontal != 0)
                transform.Rotate(0, 0, -horizontal * RotateSpeed);

            // �ƶ�
            if (vertical > 0)
            {
                MoveSpeed += Time.fixedDeltaTime * Acceleration;
                Debug.Log("��ǰ�ٶȣ�" + MoveSpeed);

                //TODO ����ȼ��
            }

            SelfRigidbody2D.velocity = transform.up * MoveSpeed;
        }
    }
}
