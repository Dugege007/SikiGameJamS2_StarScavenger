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

            DpadUp.Hide();
            DpadDown.Hide();
            DpadLeft.Hide();
            DpadRight.Hide();

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
                    GenerateHPAndShield(HeartRed.gameObject, HPHolder, hp - lastHP);
                else if (hp - lastHP < 0)
                    RemoveHPAndShield(HPHolder, -(hp - lastHP));
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
                    HPReducingText.Show();
                else
                    HPReducingText.Hide();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 燃料相关
            Global.Fuel.RegisterWithInitValue(fuel =>
            {
                FuelBar.value = (float)fuel / Global.MaxFuel.Value;
                FuelText.text = "燃料：" + fuel + "/" + Global.MaxFuel.Value;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 金币相关
            Global.Coin.RegisterWithInitValue(coin =>
            {
                CoinText.text = coin.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 运动状态相关
            Global.CurrentSpeed.RegisterWithInitValue(speed =>
            {
                SpeedSlider.value = speed / Global.MaxSpeed.Value;
                SpeedText.text = speed.ToString("0.0");

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

            // 延时关闭提示语
            ActionKit.Delay(5f, () =>
            {
                SceneTitleText.text = "流浪者星系";
                SmallTitleText.text = "开始探索";

                ControlTip.Hide();
                SceneTitleText.Hide();
                SmallTitleText.Hide();

            }).Start(this);



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
