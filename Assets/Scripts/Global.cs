using QFramework;
using UnityEngine;

namespace StarScavenger
{
    public class Global : Architecture<Global>
    {
        protected override void Init()
        {
        }

        // 时间
        public static BindableProperty<float> CurrentSeconds = new(0);

        // 基本数据
        public static BindableProperty<int> HP = new(3);
        public static BindableProperty<bool> IsReducingHP = new(false);
        public static BindableProperty<int> Shield = new(0);
        public static BindableProperty<int> Fuel = new(120);
        public static BindableProperty<int> MaxFuel = new(120);
        public static BindableProperty<int> FuelConsumpt = new(1);
        public static BindableProperty<float> FuelAutoConsumptTime = new(3f);
        public static BindableProperty<int> Coin = new(0);

        // 运动数据
        public static BindableProperty<float> MoveSpeed = new(1f);
        public static BindableProperty<float> Acceleration = new(0.5f);
        public static BindableProperty<float> PropulsiveForceValue = new(1f);
        public static BindableProperty<float> RotateSpeed = new(0.5f);
        public static BindableProperty<float> CurrentSpeed = new(1f);
        public static BindableProperty<float> MaxSpeed = new(10f);

        // 武器数据
        public static BindableProperty<float> ProjectileSpeed = new(10f);

        // 能力数据
        // 预测路径点数
        public static BindableProperty<int> PathResolution = new(20);
        // 预测路径时间长度
        public static BindableProperty<float> PathPredictTime = new(2f);
        // 是否即将碰撞
        public static BindableProperty<bool> IsAboutCollide = new(false);

        // 小行星数据
        public static BindableProperty<float> MaxGATime = new(3f);
        public static BindableProperty<float> MinGATime = new(1f);
        public static BindableProperty<bool> CanGenerate = new(true);

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // 设置 UI
            UIKit.Root.SetResolution(1920, 1080, 0.5f);
        }

        public static void ResetData()
        {
            CurrentSeconds.Value = 0;

            HP.Value = 3;
            IsReducingHP.Value = false;
            Shield.Value = 0;
            Fuel.Value = MaxFuel.Value;
            FuelConsumpt.Value = 1;
            FuelAutoConsumptTime.Value = 3f;
            Coin.Value = 0;

            MoveSpeed.Value = 1f;
            Acceleration.Value = 0.5f;
            PropulsiveForceValue.Value = 1f;
            RotateSpeed.Value = 0.5f;
            CurrentSpeed.Value = 1f;
            MaxSpeed.Value = 5f;

            ProjectileSpeed.Value = 10f;

            PathResolution.Value = 20;
            PathPredictTime.Value = 2f;
            IsAboutCollide.Value = false;

            MaxGATime.Value = 3f;
            MinGATime.Value = 1f;
            CanGenerate.Value = true;
        }
    }
}
