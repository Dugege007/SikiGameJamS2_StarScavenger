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

            // 用时
            TimeCountText.text = "用时 " + Global.EndTime.Value;

            // 星球相关
            PlanetCountText.text = Global.DiscoveredPlanetCount.Value + "/" + Global.MaxPlanet.Value;

            PlanetBestCountText.text = Global.ArrivedPlanetCount.Value + "/" + Global.MaxPlanet.Value;
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
