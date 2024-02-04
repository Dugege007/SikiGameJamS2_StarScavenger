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

        public Transform TargetPlanetTrans;
        private Camera mCamera;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as GamePanelData ?? new GamePanelData();
            // please add init code here

            Default = this;
            mCamera = Camera.main;

            // ��ʼ����
            PlanetCount.Hide();
            PlanetBestCount.Hide();

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

            ArrowIndicatorDown.Hide();
            ArrowIndicatorDownLeft.Hide();
            ArrowIndicatorDownRight.Hide();
            ArrowIndicatorLeft.Hide();
            ArrowIndicatorRight.Hide();
            ArrowIndicatorUp.Hide();
            ArrowIndicatorUpLeft.Hide();
            ArrowIndicatorUpRight.Hide();

            // ȫ�� Update
            ActionKit.OnUpdate.Register(() =>
            {
                Global.CurrentSeconds.Value += Time.deltaTime;

                Player player = Player.Default;
                if (player != null)
                {
                    if (player.NextTargetPlanet)
                    {
                        TargetPlanetTrans = player.NextTargetPlanet.transform;

                        // ��ȡ����������ת��Ŀ�귽��
                        Vector3 toTarget = mCamera.transform.InverseTransformDirection(TargetPlanetTrans.position - player.transform.position);
                        toTarget.z = 0;

                        DetermineDirection(toTarget);
                    }
                }

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

                // ��ͣ��Ϸ
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Global.IsGamePause.Value = !Global.IsGamePause.Value;
                    if (Global.IsGamePause.Value)
                    {
                        // ����ͣ��ʾ
                        DialogShow("����ͣ���ٰ� ESC ������");
                        Time.timeScale = 0;
                    }
                    else
                    {
                        // ����ͣ��ʾ
                        DialogShow("������Ϸ");
                        Time.timeScale = 1f;
                    }
                }

                //#if UNITY_EDITOR
                // ����
                if (Input.GetKeyDown(KeyCode.F1))
                    Global.HP.Value++;

                if (Input.GetKeyDown(KeyCode.F2))
                    Global.Fuel.Value += 10;

                if (Input.GetKeyDown(KeyCode.F3))

                    if (Input.GetKeyDown(KeyCode.F4))
                        Global.ArrivedPlanetCount.Value++;


                if (Input.GetKeyDown(KeyCode.F5))
                    Global.HP.Value--;

                if (Input.GetKeyDown(KeyCode.F6))
                    Global.Fuel.Value -= 10;

                if (Input.GetKeyDown(KeyCode.F7))

                    if (Input.GetKeyDown(KeyCode.F8))
                        Global.ArrivedPlanetCount.Value--;
                //#endif
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

                    if (currentSeconds % 60 == 0 && currentSecondsInt > 0)
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

            Global.AttackTimes.RegisterWithInitValue(attackTimes =>
            {
                if (attackTimes == 10)
                    DialogShow("ȼ�ϼ��ǵ�ҩ");

                if (attackTimes > 0 && attackTimes % 30 == 0)
                    DialogShow("ȼ�ϼ��ǵ�ҩ");

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

            // �������
            Global.DiscoveredPlanetCount.RegisterWithInitValue(discoveredPlanet =>
            {
                if (discoveredPlanet > 0)
                {
                    PlanetCount.Show();
                    PlanetCountText.text = discoveredPlanet + "/" + Global.MaxPlanet.Value;
                    FloatingTextController.Play("��������", TextType.Discover);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.ArrivedPlanetCount.RegisterWithInitValue(arrivedPlanet =>
            {
                if (arrivedPlanet > 0)
                {
                    PlanetBestCount.Show();
                    PlanetBestCountText.text = arrivedPlanet + "/" + Global.MaxPlanet.Value;
                    FloatingTextController.Play("��������", TextType.Arrive);
                }
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

        private bool IsTargetVisible(Vector3 toTarget)
        {
            Vector3 screenPoint = mCamera.WorldToViewportPoint(TargetPlanetTrans.position);
            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }

        private void DetermineDirection(Vector3 toTarget)
        {
            ArrowIndicatorDown.Hide();
            ArrowIndicatorDownLeft.Hide();
            ArrowIndicatorDownRight.Hide();
            ArrowIndicatorLeft.Hide();
            ArrowIndicatorRight.Hide();
            ArrowIndicatorUp.Hide();
            ArrowIndicatorUpLeft.Hide();
            ArrowIndicatorUpRight.Hide();

            // �жϷ���
            float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360; // ���Ƕ�ת��Ϊ0��360��

            if (angle > 337.5 || angle <= 22.5)
            {
                ArrowIndicatorRight.Show();
            }
            else if (angle > 22.5 && angle <= 67.5)
            {
                ArrowIndicatorUpRight.Show();
            }
            else if (angle > 67.5 && angle <= 112.5)
            {
                ArrowIndicatorUp.Show();
            }
            else if (angle > 112.5 && angle <= 157.5)
            {
                ArrowIndicatorUpLeft.Show();
            }
            else if (angle > 157.5 && angle <= 202.5)
            {
                ArrowIndicatorLeft.Show();
            }
            else if (angle > 202.5 && angle <= 247.5)
            {
                ArrowIndicatorDownLeft.Show();
            }
            else if (angle > 247.5 && angle <= 292.5)
            {
                ArrowIndicatorDown.Show();
            }
            else if (angle > 292.5 && angle <= 337.5)
            {
                ArrowIndicatorDownRight.Show();
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
