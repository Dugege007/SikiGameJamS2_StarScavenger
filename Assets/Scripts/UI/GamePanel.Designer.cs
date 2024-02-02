using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace StarScavenger
{
	// Generate Id:be36250f-e1aa-4ed6-bc5f-bccd2770adf9
	public partial class GamePanel
	{
		public const string Name = "GamePanel";
		
		[SerializeField]
		public UnityEngine.UI.Slider FuelBar;
		[SerializeField]
		public UnityEngine.UI.Text FuelText;
		[SerializeField]
		public RectTransform HPHolder;
		[SerializeField]
		public UnityEngine.UI.Image HeartRed;
		[SerializeField]
		public RectTransform ShieldHolder;
		[SerializeField]
		public UnityEngine.UI.Image HeartGreen;
		[SerializeField]
		public UnityEngine.UI.Text CoinText;
		[SerializeField]
		public UnityEngine.UI.Text SceneTitleText;
		
		private GamePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FuelBar = null;
			FuelText = null;
			HPHolder = null;
			HeartRed = null;
			ShieldHolder = null;
			HeartGreen = null;
			CoinText = null;
			SceneTitleText = null;
			
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
