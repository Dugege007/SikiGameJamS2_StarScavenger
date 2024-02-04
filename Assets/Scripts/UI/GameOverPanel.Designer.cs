using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace StarScavenger
{
	// Generate Id:444f252d-9482-41e3-adf1-356ea7879fa0
	public partial class GameOverPanel
	{
		public const string Name = "GameOverPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text GameOverText;
		[SerializeField]
		public UnityEngine.UI.Image PlanetCount;
		[SerializeField]
		public UnityEngine.UI.Text PlanetCountText;
		[SerializeField]
		public UnityEngine.UI.Image PlanetBestCount;
		[SerializeField]
		public UnityEngine.UI.Text PlanetBestCountText;
		[SerializeField]
		public UnityEngine.UI.Button BtnRestart;
		[SerializeField]
		public UnityEngine.UI.Button BtnBackToMenu;
		
		private GameOverPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			GameOverText = null;
			PlanetCount = null;
			PlanetCountText = null;
			PlanetBestCount = null;
			PlanetBestCountText = null;
			BtnRestart = null;
			BtnBackToMenu = null;
			
			mData = null;
		}
		
		public GameOverPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		GameOverPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new GameOverPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
