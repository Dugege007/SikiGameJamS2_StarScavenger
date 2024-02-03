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
                    // 基于玩家当前速度计算生成角度范围
                    // 速度越快，角度范围越小，最小不低于 45 度
                    float angleRange = Mathf.Max(180 - Global.CurrentSpeed.Value * 20f, 24f);

                    // 获取玩家前进方向的角度
                    Vector2 velocityDirection = player.SelfRigidbody2D.velocity.normalized;
                    float playerAngle = Mathf.Atan2(velocityDirection.y, velocityDirection.x) * Mathf.Rad2Deg;

                    // 计算随机生成角度的范围
                    float minAngle = playerAngle - angleRange / 2;
                    float maxAngle = playerAngle + angleRange / 2;

                    // 随机选择一个角度
                    float randomAngle = Random.Range(minAngle, maxAngle);
                    // 弧度
                    float radian = randomAngle * Mathf.Deg2Rad;

                    Vector3 cameraPos = CameraController.Default.transform.position;
                    float randomRadius = Random.Range(MinJunkGRadius, MaxJunkGRadius);
                    // 极坐标确定点的位置
                    Vector3 pos = new Vector3(
                        cameraPos.x + randomRadius * Mathf.Cos(radian),
                        cameraPos.y + randomRadius * Mathf.Sin(radian),
                        cameraPos.z + 10f);

                    // 生成垃圾
                    int randomIndex = Random.Range(0, Asteroids.Count);
                    Asteroids[randomIndex].gameObject.InstantiateWithParent(this)
                        .Position(pos)
                        .Show();
                }
            }
        }
    }
}
