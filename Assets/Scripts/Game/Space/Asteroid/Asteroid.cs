using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public class Asteroid : MonoBehaviour
    {
        public float MinMoveSpeed = 0f;
        public float MaxMoveSpeed = 3f;
        private Rigidbody2D rigid2D;

        public float DefaultExistTime = 5f;
        private float mCurrentETime = 0;

        private void Awake()
        {
            rigid2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // �����������
            float randomAngle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

            // ��������ٶ�
            float randomSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);

            // ����С���ǵ��ٶ�
            rigid2D.velocity = direction * randomSpeed;
        }

        private void Update()
        {
            Player player = Player.Default;
            if (player != null)
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= 21f)
                {
                    mCurrentETime = 0;
                    return;
                }
            }

            mCurrentETime += Time.deltaTime;

            if (mCurrentETime > DefaultExistTime)
            {
                this.DestroyGameObjGracefully();
            }
        }
    }
}
