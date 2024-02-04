using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace StarScavenger
{
	// Generate Id:08d35035-f432-48eb-aa34-c3596f62cee8
	public partial class GamePassPanel
	{
		public const string Name = "GamePassPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text GamePassText;
		[SerializeField]
		public UnityEngine.UI.Image PlanetCount;
		[SerializeField]
		public UnityEngine.UI.Text PlanetCountText;
		[SerializeField]
		public UnityEngine.UI.Image PlanetBestCount;
		[SerializeField]
		public UnityEngine.UI.Text PlanetBestCountText;
		[SerializeField]
		public UnityEngine.UI.Text TimeCountText;
		[SerializeField]
		public UnityEngine.UI.Button BtnRestart;
		[SerializeField]
		public UnityEngine.UI.Button BtnBackToMenu;
		
		private GamePassPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			GamePassText = null;
			PlanetCount = null;
			PlanetCountText = null;
			PlanetBestCount = null;
			PlanetBestCountText = null;
			TimeCountText = null;
			BtnRestart = null;
			BtnBackToMenu = null;
			
			mData = null;
		}
		
		public GamePassPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		GamePassPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new GamePassPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
