using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;

namespace StarScavenger
{
    public class GamePanelData : UIPanelData
    {
    }
    public partial class GamePanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as GamePanelData ?? new GamePanelData();
            // please add init code here

            HeartRed.Hide();
            HeartGreen.Hide();
            SceneTitleText.Hide();

            int lastHP = Global.HP.Value;
            int lastShield = Global.Shield.Value;

            GenerateHPAndShield(HeartRed.gameObject, HPHolder, Global.HP.Value);

            if (Global.Shield.Value > 0)
                GenerateHPAndShield(HeartGreen.gameObject, ShieldHolder, Global.Shield.Value);

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

            Global.Fuel.RegisterWithInitValue(fuel =>
            {
                FuelBar.value = (float)fuel / Global.MaxFuel.Value;
                FuelText.text = "È¼ÁÏ£º" + fuel + "/" + Global.MaxFuel.Value;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Coin.RegisterWithInitValue(coin =>
            {
                CoinText.text = coin.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentSpeed.RegisterWithInitValue(speed =>
            {
                SpeedSlider.value = speed / Global.MaxSpeed.Value;
                SpeedText.text = speed.ToString("0.0");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ²âÊÔ
            BtnAddHP.onClick.AddListener(() =>
            {
                Global.HP.Value++;
            });
            BtnRemoveHP.onClick.AddListener(() =>
            {
                Global.HP.Value--;
            });
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
        }
    }
}
