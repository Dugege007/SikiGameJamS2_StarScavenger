using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;

namespace StarScavenger
{
    public class GameStartPanelData : UIPanelData
    {
    }
    public partial class GameStartPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as GameStartPanelData ?? new GameStartPanelData();
            // please add init code here

            BtnStartGame.onClick.AddListener(() =>
            {
                Global.ResetData();

                CloseSelf();
                SceneManager.LoadScene("Game");
            });

            BtnQuit.onClick.AddListener(() =>
            {
                Application.Quit();
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
