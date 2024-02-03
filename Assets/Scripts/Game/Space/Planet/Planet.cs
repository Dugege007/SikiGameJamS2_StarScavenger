using UnityEngine;
using QFramework;
using UnityEngine.UI;

namespace StarScavenger
{
    public enum PlanetType
    {
        Ice,
        Ocean,
        Ring,
        Forest,
        Earth,
        Tech,
        Sun,
    }

    public partial class Planet : ViewController
    {
        public bool IsDiscover = false;
        public bool IsArrived = false;
        public PlanetType Type;
        public bool IsRotate = true;
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
            GravityArea.OnTriggerEnter2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    Player player = Player.Default;
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        Text title = GamePanel.Default.SceneTitleText;
                        Text description = GamePanel.Default.SmallTitleText;

                        switch (Type)
                        {
                            case PlanetType.Ice:
                                title.text = "����";
                                break;
                            case PlanetType.Ocean:
                                title.text = "����";
                                break;
                            case PlanetType.Ring:
                                title.text = "����";
                                break;
                            case PlanetType.Forest:
                                title.text = "����";
                                break;
                            case PlanetType.Earth:
                                title.text = "����";
                                break;
                            case PlanetType.Tech:
                                title.text = "����";
                                break;
                            case PlanetType.Sun:
                                title.text = "����";
                                break;
                            default:
                                break;
                        }

                        description.text = "�ӽ���";

                        title.Show()
                            .Delay(3f, () =>
                            {
                                title.Hide();
                            }).Execute();

                        description.Show()
                            .Delay(3f, () =>
                            {
                                description.Hide();
                            }).Execute();
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            GravityArea.OnTriggerStay2DEvent(collider2D =>
            {
                HitHurtBox hitHurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hitHurtBox != null)
                {
                    Player player = Player.Default;
                    if (hitHurtBox.Owner.CompareTag("Player"))
                    {
                        player.GravityEffect(this);
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
            if (IsRotate)
            {
                transform.Rotate(0, 0, mRandomRotationSpeed);
            }
        }
    }
}
