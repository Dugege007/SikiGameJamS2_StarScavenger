using QFramework;
using UnityEngine.SceneManagement;

namespace StarScavenger
{
	public class GameOverPanelData : UIPanelData
	{
	}
	public partial class GameOverPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as GameOverPanelData ?? new GameOverPanelData();
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
