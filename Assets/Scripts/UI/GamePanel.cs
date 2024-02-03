using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public class GamePanelData : UIPanelData
    {
    }
    public partial class GamePanel : UIPanel
    {
        public static GamePanel Default;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as GamePanelData ?? new GamePanelData();
            // please add init code here

            Default = this;

            // ��ʼ����
            HeartRed.Hide();
            HeartGreen.Hide();
            HPReducingText.Hide();
            AccelerationDownText.Hide();
            ControlTip.Hide();

            DpadUp.Hide();
            DpadDown.Hide();
            DpadLeft.Hide();
            DpadRight.Hide();

            DialogText.Hide();

            // ȫ�� Update
            ActionKit.OnUpdate.Register(() =>
            {
                Global.CurrentSeconds.Value += Time.deltaTime;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ���µ�ǰʱ��
            Global.CurrentSeconds.RegisterWithInitValue(currentSeconds =>
            {
                // ÿ 20 ֡����һ��
                if (Time.frameCount % 20 == 0)
                {
                    int currentSecondsInt = Mathf.FloorToInt(currentSeconds);
                    int seconds = currentSecondsInt % 60;
                    int minutes = currentSecondsInt / 60;
                    TimeText.text = $"{minutes:00}:{seconds:00}";

                    if (currentSecondsInt % 60 == 0 && currentSecondsInt > 0)
                    {
                        DialogShow("ʱ��ʱ����Щ��~");
                    }
                }


            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ���� HP �� Shield UI
            GenerateHPAndShield(HeartRed.gameObject, HPHolder, Global.HP.Value);
            if (Global.Shield.Value > 0)
                GenerateHPAndShield(HeartGreen.gameObject, ShieldHolder, Global.Shield.Value);

            // �����������
            int lastHP = Global.HP.Value;
            int lastShield = Global.Shield.Value;

            Global.HP.RegisterWithInitValue(hp =>
            {
                if (hp - lastHP > 0)
                {
                    GenerateHPAndShield(HeartRed.gameObject, HPHolder, hp - lastHP);
                    DialogShow("���������");
                }
                else if (hp - lastHP < 0)
                {
                    RemoveHPAndShield(HPHolder, -(hp - lastHP));
                    DialogShow("�ۣ�");
                }

                lastHP = hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Shield.RegisterWithInitValue(shield =>
            {
                if (shield - lastShield > 0)
                    GenerateHPAndShield(HeartGreen.gameObject, ShieldHolder, shield - lastShield);
                else if (shield - lastShield < 0)
                    RemoveHPAndShield(ShieldHolder, -(shield - lastShield));
                lastShield = shield;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.IsReducingHP.RegisterWithInitValue(isReducingHP =>
            {
                if (isReducingHP)
                {
                    HPReducingText.Show();
                    DialogShow("���˶�ǽ����ǽ...");
                }
                else
                {
                    HPReducingText.Hide();
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ȼ�����
            int lastFuel = Global.Fuel.Value;

            Global.Fuel.RegisterWithInitValue(fuel =>
            {
                FuelBar.value = (float)fuel / Global.MaxFuel.Value;
                FuelText.text = "ȼ�ϣ�" + fuel + "/" + Global.MaxFuel.Value;

                if (lastFuel - fuel > 0)
                {
                    if (fuel == 10)
                    {
                        if (Player.Default.CanAttack)
                            DialogShow("ȼ��û�ˣ�Ҫ����...");
                        else
                            DialogShow("ϣ��������ǰ��");
                    }
                    else if (fuel == 50)
                        DialogShow("ע��ȼ�ϣ�");
                }
                else if (lastFuel - fuel < 0)
                {
                    if (Player.Default.CanAttack)
                    {
                        if (fuel == Global.MaxFuel.Value)
                            DialogShow("���������İ�ȫ��~");
                    }
                    if (fuel == 10)
                        DialogShow("���� �ٳ�һ��...");
                    else if (fuel == 50)
                        DialogShow("��Դ����");
                }

                lastFuel = fuel;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ������
            Global.Coin.RegisterWithInitValue(coin =>
            {
                CoinText.text = coin.ToString();

                if (coin % 20 == 0 && coin > 0)
                    DialogShow("�Ұ���ң�");
                if (coin == 66)
                    DialogShow("������˳��");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // �˶�״̬���
            Global.CurrentSpeed.RegisterWithInitValue(speed =>
            {
                SpeedSlider.value = speed / Global.MaxSpeed.Value;
                SpeedText.text = speed.ToString("0.0");

                if (speed == 5)
                    DialogShow("�ٶ�Խ�죬��ȼ��Խ��");

                if (speed == 10)
                    DialogShow("��Ҫ��������");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ȫ�� Update ��������
            ActionKit.OnUpdate.Register(() =>
            {
                // �ƶ�������ʾ
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                    DpadUp.Show();
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    DpadDown.Show();
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    DpadLeft.Show();
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    DpadRight.Show();

                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                    DpadUp.Hide();
                if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                    DpadDown.Hide();
                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
                    DpadLeft.Hide();
                if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                    DpadRight.Hide();

                // ������ʾ����
                if (Input.GetKeyDown(KeyCode.H))
                    ControlTip.Show();
                if (Input.GetKeyUp(KeyCode.H))
                    ControlTip.Hide();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��ʾ��
            SceneTitleText.text = "��������ϵ";
            SceneTitleText.Show().Delay(3f, () =>
            {
                SceneTitleText.Hide();

            }).Execute();

            DescriptionShow("��ʼ̽��");

            DialogShow("������һ����ϵ");

#if UNITY_EDITOR
            // ����
            BtnAddHP.onClick.AddListener(() =>
            {
                Global.HP.Value++;
            });
            BtnRemoveHP.onClick.AddListener(() =>
            {
                Global.HP.Value--;
            });
            BtnAddFuel.onClick.AddListener(() =>
            {
                Global.Fuel.Value += 10;
            });
            BtnRemoveFuel.onClick.AddListener(() =>
            {
                Global.Fuel.Value -= 10;
            });
#endif
        }

        private void GenerateHPAndShield(GameObject needG, Transform parent, int needNum)
        {
            for (int i = 0; i < needNum; i++)
            {
                needG.InstantiateWithParent(parent)
                    .SiblingIndex(0)
                    .Show();
            }
        }

        private void RemoveHPAndShield(Transform parent, int needNum)
        {
            for (int i = 0; i < needNum; i++)
            {
                if (parent.childCount <= 1) return;
                parent.GetChild(0).gameObject.DestroySelfGracefully();
            }
        }

        public void DialogShow(string dialog)
        {
            DialogText.InstantiateWithParent(DialogHolder.transform)
                .Self(self => self.text = dialog)
                .Show()
                .DestroyGameObjAfterDelayGracefully(3f);
        }

        public void DescriptionShow(string description)
        {
            SmallTitleText.text = description;
            SmallTitleText.Show().Delay(3f, () =>
            {
                SmallTitleText.Hide();
            }).Execute();
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
            Default = null;
        }
    }
}
