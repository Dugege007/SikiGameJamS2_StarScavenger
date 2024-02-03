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

            // ��ҽ���С���Ǵ�
            AsteroidArea1.OnTriggerEnter2DEvent(collider2D =>
            {
                SetEnterAsteroidAreaCollider(collider2D);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            AsteroidArea2.OnTriggerEnter2DEvent(collider2D =>
            {
                SetEnterAsteroidAreaCollider(collider2D);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ����뿪С���Ǵ�
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
                    // ������ҵ�ǰ�ٶȼ������ɽǶȷ�Χ
                    // �ٶ�Խ�죬�Ƕȷ�ΧԽС����С������ 45 ��
                    float angleRange = Mathf.Max(150f - Global.CurrentSpeed.Value * 20f, 24f);

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

                    title.text = "С���Ǵ�";
                    description.text = "С������������";

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
