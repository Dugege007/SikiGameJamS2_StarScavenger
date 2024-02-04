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

            // 初始隐藏
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

            // 全局 Update
            ActionKit.OnUpdate.Register(() =>
            {
                Global.CurrentSeconds.Value += Time.deltaTime;

                Player player = Player.Default;
                if (player != null)
                {
                    if (player.NextTargetPlanet)
                    {
                        TargetPlanetTrans = player.NextTargetPlanet.transform;

                        // 获取相对于相机旋转的目标方向
                        Vector3 toTarget = mCamera.transform.InverseTransformDirection(TargetPlanetTrans.position - player.transform.position);
                        toTarget.z = 0;

                        DetermineDirection(toTarget);
                    }
                }

                // 移动方向提示
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

                // 操作提示开关
                if (Input.GetKeyDown(KeyCode.H))
                    ControlTip.Show();
                if (Input.GetKeyUp(KeyCode.H))
                    ControlTip.Hide();

                // 暂停游戏
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Global.IsGamePause.Value = !Global.IsGamePause.Value;
                    if (Global.IsGamePause.Value)
                    {
                        // 打开暂停提示
                        DialogShow("已暂停，再按 ESC 键继续");
                        Time.timeScale = 0;
                    }
                    else
                    {
                        // 打开暂停提示
                        DialogShow("继续游戏");
                        Time.timeScale = 1f;
                    }
                }

                //#if UNITY_EDITOR
                // 测试
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

            // 更新当前时间
            Global.CurrentSeconds.RegisterWithInitValue(currentSeconds =>
            {
                // 每 20 帧更新一次
                if (Time.frameCount % 20 == 0)
                {
                    int currentSecondsInt = Mathf.FloorToInt(currentSeconds);
                    int seconds = currentSecondsInt % 60;
                    int minutes = currentSecondsInt / 60;
                    TimeText.text = $"{minutes:00}:{seconds:00}";

                    if (currentSeconds % 60 == 0 && currentSecondsInt > 0)
                    {
                        DialogShow("时光时光慢些吧~");
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 生成 HP 和 Shield UI
            GenerateHPAndShield(HeartRed.gameObject, HPHolder, Global.HP.Value);
            if (Global.Shield.Value > 0)
                GenerateHPAndShield(HeartGreen.gameObject, ShieldHolder, Global.Shield.Value);

            // 基础属性相关
            int lastHP = Global.HP.Value;
            int lastShield = Global.Shield.Value;

            Global.HP.RegisterWithInitValue(hp =>
            {
                if (hp - lastHP > 0)
                {
                    GenerateHPAndShield(HeartRed.gameObject, HPHolder, hp - lastHP);
                    DialogShow("精神焕发！");
                }
                else if (hp - lastHP < 0)
                {
                    RemoveHPAndShield(HPHolder, -(hp - lastHP));
                    DialogShow("疼！");
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
                    DialogShow("拆了东墙补西墙...");
                }
                else
                {
                    HPReducingText.Hide();
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 燃料相关
            int lastFuel = Global.Fuel.Value;

            Global.Fuel.RegisterWithInitValue(fuel =>
            {
                FuelBar.value = (float)fuel / Global.MaxFuel.Value;
                FuelText.text = "燃料：" + fuel + "/" + Global.MaxFuel.Value;

                if (lastFuel - fuel > 0)
                {
                    if (fuel == 10)
                    {
                        if (Player.Default.CanAttack)
                            DialogShow("燃料没了，要完了...");
                        else
                            DialogShow("希望就在眼前！");
                    }
                    else if (fuel == 50)
                        DialogShow("注意燃料！");
                }
                else if (lastFuel - fuel < 0)
                {
                    if (Player.Default.CanAttack)
                    {
                        if (fuel == Global.MaxFuel.Value)
                            DialogShow("油箱满满的安全感~");
                    }
                    if (fuel == 10)
                        DialogShow("还能 再撑一下...");
                    else if (fuel == 50)
                        DialogShow("开源节流");
                }

                lastFuel = fuel;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.AttackTimes.RegisterWithInitValue(attackTimes =>
            {
                if (attackTimes == 10)
                    DialogShow("燃料即是弹药");

                if (attackTimes > 0 && attackTimes % 30 == 0)
                    DialogShow("燃料即是弹药");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 金币相关
            Global.Coin.RegisterWithInitValue(coin =>
            {
                CoinText.text = coin.ToString();

                if (coin % 20 == 0 && coin > 0)
                    DialogShow("我爱金币！");
                if (coin == 66)
                    DialogShow("六六大顺！");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 运动状态相关
            Global.CurrentSpeed.RegisterWithInitValue(speed =>
            {
                SpeedSlider.value = speed / Global.MaxSpeed.Value;
                SpeedText.text = speed.ToString("0.0");

                if (speed == 5)
                    DialogShow("速度越快，耗燃料越快");

                if (speed == 10)
                    DialogShow("想要超光速吗？");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 星球相关
            Global.DiscoveredPlanetCount.RegisterWithInitValue(discoveredPlanet =>
            {
                if (discoveredPlanet > 0)
                {
                    PlanetCount.Show();
                    PlanetCountText.text = discoveredPlanet + "/" + Global.MaxPlanet.Value;
                    FloatingTextController.Play("发现星球", TextType.Discover);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.ArrivedPlanetCount.RegisterWithInitValue(arrivedPlanet =>
            {
                if (arrivedPlanet > 0)
                {
                    PlanetBestCount.Show();
                    PlanetBestCountText.text = arrivedPlanet + "/" + Global.MaxPlanet.Value;
                    FloatingTextController.Play("到达星球", TextType.Arrive);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 提示语
            SceneTitleText.text = "流浪者星系";
            SceneTitleText.Show().Delay(3f, () =>
            {
                SceneTitleText.Hide();

            }).Execute();

            DescriptionShow("开始探索");

            DialogShow("啊，又一个星系");

#if UNITY_EDITOR
            // 测试
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

            // 判断方向
            float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360; // 将角度转换为0到360度

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
