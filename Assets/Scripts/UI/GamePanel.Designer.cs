using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace StarScavenger
{
	// Generate Id:b72293cc-d5e1-4b0c-8440-00248f4d2130
	public partial class GamePanel
	{
		public const string Name = "GamePanel";
		
		[SerializeField]
		public RectTransform ControlTip;
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
		public UnityEngine.UI.Text HPReducingText;
		[SerializeField]
		public UnityEngine.UI.Text CoinText;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddHP;
		[SerializeField]
		public UnityEngine.UI.Button BtnRemoveHP;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddFuel;
		[SerializeField]
		public UnityEngine.UI.Button BtnRemoveFuel;
		[SerializeField]
		public UnityEngine.UI.Text SceneTitleText;
		[SerializeField]
		public UnityEngine.UI.Text SmallTitleText;
		[SerializeField]
		public UnityEngine.UI.Slider SpeedSlider;
		[SerializeField]
		public UnityEngine.UI.Text SpeedText;
		[SerializeField]
		public UnityEngine.UI.Button DpadRight;
		[SerializeField]
		public UnityEngine.UI.Button DpadDown;
		[SerializeField]
		public UnityEngine.UI.Button DpadLeft;
		[SerializeField]
		public UnityEngine.UI.Button DpadUp;
		
		private GamePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ControlTip = null;
			FuelBar = null;
			FuelText = null;
			HPHolder = null;
			HeartRed = null;
			ShieldHolder = null;
			HeartGreen = null;
			HPReducingText = null;
			CoinText = null;
			BtnAddHP = null;
			BtnRemoveHP = null;
			BtnAddFuel = null;
			BtnRemoveFuel = null;
			SceneTitleText = null;
			SmallTitleText = null;
			SpeedSlider = null;
			SpeedText = null;
			DpadRight = null;
			DpadDown = null;
			DpadLeft = null;
			DpadUp = null;
			
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
