using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Planet : ViewController
    {
        public float MinRotationSpeed = 0.1f;
        public float MaxRotationSpeed = 1f;
        // ���������������ٶ�
        public float Gravity = 10f;
        public float Radius = 1.5f;
        private float mRandomRotationSpeed;

        private void Start()
        {
            Radius = HitHurtBox.radius;

            mRandomRotationSpeed = Random.Range(MinRotationSpeed, MaxRotationSpeed);

            // ����Ӱ������
            GravityArea.OnTriggerStay2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    Player player = Player.Default;
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        player.GravityEffect(this);
                        //TODO ��ʾ����������
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            GravityArea.OnTriggerExit2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    Player player = Player.Default;
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        player.GravityEffect(null);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��ֹ��������
            HoldFireArea.OnTriggerEnter2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    Player player = Player.Default;
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        // ����������
                        player.LineRenderer1.Show();
                        player.CanAttack = false;
                        //TODO ��ʾ���������
                        //TODO �ر���������
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            HoldFireArea.OnTriggerExit2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    Player player = Player.Default;
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        player.LineRenderer1.Hide();
                        player.CanAttack = true;
                        //TODO ������������
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // �Զ�����/��������
            DefenseArea.OnTriggerEnter2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        //TODO ��ʾ���������
                        //TODO ��ý���
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            DefenseArea.OnTriggerStay2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        //TODO ����һ��ʱ�������ɾ�
                    }
                    if (hitHurtBox.Owner.CompareTag("Asteroid"))
                    {
                        //TODO ��������
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            DefenseArea.OnTriggerExit2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        //TODO ��ʱ���뿪��������ʾ��������
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            transform.Rotate(0, 0, mRandomRotationSpeed);
        }
    }
}
