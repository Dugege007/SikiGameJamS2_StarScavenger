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

            // 初始隐藏
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

            // 全局 Update
            ActionKit.OnUpdate.Register(() =>
            {
                Global.CurrentSeconds.Value += Time.deltaTime;

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

                    if (currentSecondsInt % 60 == 0 && currentSecondsInt > 0)
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

            // 全局 Update 监听按键
            ActionKit.OnUpdate.Register(() =>
            {
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
