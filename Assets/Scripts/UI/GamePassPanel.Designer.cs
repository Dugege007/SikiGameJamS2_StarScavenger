using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace StarScavenger
{
	// Generate Id:67c9366b-529b-4482-9bfd-5bd86b1a6bb2
	public partial class GamePassPanel
	{
		public const string Name = "GamePassPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text GamePassText;
		[SerializeField]
		public UnityEngine.UI.Button BtnRestart;
		[SerializeField]
		public UnityEngine.UI.Button BtnBackToMenu;
		
		private GamePassPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			GamePassText = null;
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
