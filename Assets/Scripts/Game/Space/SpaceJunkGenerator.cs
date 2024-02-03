using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace StarScavenger
{
    public partial class SpaceJunkGenerator : ViewController
    {
        public List<Asteroid> Asteroids = new List<Asteroid>();

        public float MaxJunkGTime = 3f;
        public float MinJunkGTime = 1f;

        private float mNextGtime;
        private float mCurrentGTime = 0;

        public float MinJunkGRadius = 21f;
        public float MaxJunkGRadius = 25f;

        private void Start()
        {
            foreach (var asteroid in Asteroids)
                asteroid.Hide();

            mNextGtime = Random.Range(MinJunkGTime, MaxJunkGTime);
        }

        private void Update()
        {
            mCurrentGTime += Time.deltaTime;

            if (mCurrentGTime > mNextGtime)
            {
                mCurrentGTime = 0;
                mNextGtime = Random.Range(MinJunkGTime, MaxJunkGTime);

                Player player = Player.Default;
                if (player != null)
                {
                    // ������ҵ�ǰ�ٶȼ������ɽǶȷ�Χ
                    // �ٶ�Խ�죬�Ƕȷ�ΧԽС����С������ 45 ��
                    float angleRange = Mathf.Max(180 - Global.CurrentSpeed.Value * 20f, 24f);

                    // ��ȡ���ǰ������ĽǶ�
                    Vector2 velocityDirection = player.SelfRigidbody2D.velocity.normalized;
                    float playerAngle = Mathf.Atan2(velocityDirection.y, velocityDirection.x) * Mathf.Rad2Deg;

                    // ����������ɽǶȵķ�Χ
                    float minAngle = playerAngle - angleRange / 2;
                    float maxAngle = playerAngle + angleRange / 2;

                    // ���ѡ��һ���Ƕ�
                    float randomAngle = Random.Range(minAngle, maxAngle);
                    // ����
                    float radian = randomAngle * Mathf.Deg2Rad;

                    Vector3 cameraPos = CameraController.Default.transform.position;
                    float randomRadius = Random.Range(MinJunkGRadius, MaxJunkGRadius);
                    // ������ȷ�����λ��
                    Vector3 pos = new Vector3(
                        cameraPos.x + randomRadius * Mathf.Cos(radian),
                        cameraPos.y + randomRadius * Mathf.Sin(radian),
                        cameraPos.z + 10f);

                    // ��������
                    int randomIndex = Random.Range(0, Asteroids.Count);
                    Asteroids[randomIndex].gameObject.InstantiateWithParent(this)
                        .Position(pos)
                        .Show();
                }
            }
        }
    }
}
