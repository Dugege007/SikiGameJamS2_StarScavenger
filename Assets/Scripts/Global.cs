using QFramework;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace StarScavenger
{
    public  class Global: Architecture<Global>
    {
        protected override void Init()
        {
        }

        // 基本数据
        public static BindableProperty<int> HP = new(3);
        public static BindableProperty<int> Shield = new(1);
        public static BindableProperty<int> Fuel = new(200);
        public static BindableProperty<int> MaxFuel = new(200);
        public static BindableProperty<int> FuelConsumption = new(1);
        public static BindableProperty<int> Coin = new(0);

        // 运动数据
        public static BindableProperty<float> MoveSpeed = new(1f);
        public static BindableProperty<float> Acceleration = new(0.5f);
        public static BindableProperty<float> PropulsiveForceValue = new(1f);
        public static BindableProperty<float> RotateSpeed = new(0.5f);

        // 武器数据
        public static BindableProperty<float> ProjectileSpeed = new(10f);

        // 能力数据
        // 预测路径点数
        public static BindableProperty<int> PathResolution = new(20);
        // 预测路径时间长度
        public static BindableProperty<float> PathPredictTime = new(2f);

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // 设置 UI
            UIKit.Root.SetResolution(1920, 1080, 0.5f);
        }

        public static void ResetData()
        {
            HP.Value = 3;
            Shield.Value = 1;
            Fuel.Value = MaxFuel.Value;
            FuelConsumption.Value = 1;
            Coin.Value = 0;

            MoveSpeed.Value = 1f;
            Acceleration.Value = 0.5f;
            PropulsiveForceValue.Value = 1f;
            RotateSpeed.Value = 0.5f;

            ProjectileSpeed.Value = 10f;

            PathResolution.Value = 20;
            PathPredictTime.Value = 2f;
        }
    }
}
