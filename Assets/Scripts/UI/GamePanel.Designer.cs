using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace StarScavenger
{
	// Generate Id:09b62d31-8ffc-4fbd-83c3-fd4a448de0fb
	public partial class GamePanel
	{
		public const string Name = "GamePanel";
		
		[SerializeField]
		public UnityEngine.UI.Text FuelText;
		[SerializeField]
		public UnityEngine.UI.Text HPText;
		
		private GamePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FuelText = null;
			HPText = null;
			
			mData = null;
		}
		
		public GamePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		GamePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new GamePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
