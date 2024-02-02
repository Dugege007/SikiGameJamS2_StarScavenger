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

        public float MinJunkGRadius = 18f;
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

                if (Player.Default != null)
                {
                    // ��ȡ��Ļ��Ե֮������λ��
                    // ���ѡ��һ���Ƕ�
                    float randomAngle = Random.Range(0f, 360f);
                    // ����
                    float radian = randomAngle * Mathf.Deg2Rad;

                    Vector3 cameraPos = Camera.Default.transform.position;
                    float randomRadius = Random.Range(MinJunkGRadius, MaxJunkGRadius);
                    // ʹ�ü�����ȷ�����λ��
                    Vector3 pos = new Vector3(
                        cameraPos.x + randomRadius * Mathf.Cos(radian),
                        cameraPos.y + randomRadius * Mathf.Sin(radian),
                        cameraPos.z + 10f
                    );

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
