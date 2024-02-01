using UnityEngine;
using UnityEngine.UI;
using QFramework;

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

            Global.HP.RegisterWithInitValue(hp =>
            {
                HPText.text = hp + "/" + Global.MaxHP;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Fuel.RegisterWithInitValue(fuel =>
            {
                FuelText.text = fuel + "/" + Global.MaxFuel;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
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
