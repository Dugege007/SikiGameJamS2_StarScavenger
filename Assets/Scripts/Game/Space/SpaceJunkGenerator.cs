using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace StarScavenger
{
    public partial class SpaceJunkGenerator : ViewController
    {
        public List<Asteroid> Asteroids = new List<Asteroid>();

        private float mMaxJunkGTime;
        private float mMinJunkGTime;

        private float mNextGtime;
        private float mCurrentGTime = 0;

        public float MinJunkGRadius = 21f;
        public float MaxJunkGRadius = 25f;

        private void Start()
        {
            foreach (var asteroid in Asteroids)
                asteroid.Hide();

            mMaxJunkGTime = Global.MaxGATime.Value;
            mMinJunkGTime = Global.MinGATime.Value;
            mNextGtime = Random.Range(mMinJunkGTime, mMaxJunkGTime);

            // 玩家进入小行星带
            AsteroidArea1.OnTriggerEnter2DEvent(collider2D =>
            {
                SetEnterAsteroidAreaCollider(collider2D);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            AsteroidArea2.OnTriggerEnter2DEvent(collider2D =>
            {
                SetEnterAsteroidAreaCollider(collider2D);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 玩家离开小行星带
            AsteroidArea1.OnTriggerExit2DEvent(collider2D =>
            {
                SetExitAsteroidAreaCollider(collider2D);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            AsteroidArea2.OnTriggerExit2DEvent(collider2D =>
            {
                SetExitAsteroidAreaCollider(collider2D);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (Global.CanGenerate.Value == false) return;

            mCurrentGTime += Time.deltaTime;

            if (mCurrentGTime > mNextGtime)
            {
                mCurrentGTime = 0;
                mNextGtime = Random.Range(mMinJunkGTime, mMaxJunkGTime);

                Player player = Player.Default;
                if (player != null)
                {
                    // 基于玩家当前速度计算生成角度范围
                    // 速度越快，角度范围越小，最小不低于 45 度
                    float angleRange = Mathf.Max(150f - Global.CurrentSpeed.Value * 20f, 24f);

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

        private void SetEnterAsteroidAreaCollider(Collider2D collider2D)
        {
            HitHurtBox hitHurtBox = collider2D.GetComponentInChildren<HitHurtBox>();

            if (hitHurtBox != null)
            {
                if (hitHurtBox.Owner.CompareTag("Player"))
                {
                    mMaxJunkGTime *= 0.5f;
                    mMinJunkGTime *= 0.5f;

                    Text title = GamePanel.Default.SceneTitleText;
                    Text description = GamePanel.Default.SmallTitleText;

                    title.text = "小行星带";
                    description.text = "小行星数量增加";

                    title.Show().Delay(3f, () =>
                    {
                        title.Hide();
                    }).Execute();

                    description.Show().Delay(3f, () =>
                    {
                        description.Hide();
                    }).Execute();
                }
            }
        }

        private void SetExitAsteroidAreaCollider(Collider2D collider2D)
        {
            HitHurtBox hitHurtBox = collider2D.GetComponentInChildren<HitHurtBox>();

            if (hitHurtBox != null)
            {
                if (hitHurtBox.Owner.CompareTag("Player"))
                {
                    mMaxJunkGTime = Global.MaxGATime.Value;
                    mMinJunkGTime = Global.MinGATime.Value;
                }
            }
        }
    }
}
