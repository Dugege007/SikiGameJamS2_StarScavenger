using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace StarScavenger
{
	// Generate Id:2a4b95d7-7bd1-4875-8d93-11e7108190b0
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
		public UnityEngine.UI.Image PlanetCount;
		[SerializeField]
		public UnityEngine.UI.Text PlanetCountText;
		[SerializeField]
		public UnityEngine.UI.Image PlanetBestCount;
		[SerializeField]
		public UnityEngine.UI.Text PlanetBestCountText;
		[SerializeField]
		public UnityEngine.UI.Text TimeText;
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
		public UnityEngine.UI.Text AccelerationDownText;
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
		[SerializeField]
		public RectTransform DialogHolder;
		[SerializeField]
		public UnityEngine.UI.Text DialogText;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorUp;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorUpLeft;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorUpRight;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorLeft;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorRight;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorDown;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorDownLeft;
		[SerializeField]
		public UnityEngine.UI.Image ArrowIndicatorDownRight;
		
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
			PlanetCount = null;
			PlanetCountText = null;
			PlanetBestCount = null;
			PlanetBestCountText = null;
			TimeText = null;
			BtnAddHP = null;
			BtnRemoveHP = null;
			BtnAddFuel = null;
			BtnRemoveFuel = null;
			SceneTitleText = null;
			SmallTitleText = null;
			SpeedSlider = null;
			AccelerationDownText = null;
			SpeedText = null;
			DpadRight = null;
			DpadDown = null;
			DpadLeft = null;
			DpadUp = null;
			DialogHolder = null;
			DialogText = null;
			ArrowIndicatorUp = null;
			ArrowIndicatorUpLeft = null;
			ArrowIndicatorUpRight = null;
			ArrowIndicatorLeft = null;
			ArrowIndicatorRight = null;
			ArrowIndicatorDown = null;
			ArrowIndicatorDownLeft = null;
			ArrowIndicatorDownRight = null;
			
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
