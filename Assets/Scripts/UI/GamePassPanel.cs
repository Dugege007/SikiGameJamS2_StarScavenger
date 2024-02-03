using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;

namespace StarScavenger
{
    public class GamePassPanelData : UIPanelData
    {
    }
    public partial class GamePassPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as GamePassPanelData ?? new GamePassPanelData();
            // please add init code here

            BtnBackToMenu.onClick.AddListener(() =>
            {
                Global.ResetData();
                Player.Default.DestroyGameObjGracefully();
                CloseSelf();
                SceneManager.LoadScene("GameStart");
            });

            BtnRestart.onClick.AddListener(() =>
            {
                Global.ResetData();
                Player.Default.DestroyGameObjGracefully();
                CloseSelf();
                SceneManager.LoadScene("Game");
            });
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
