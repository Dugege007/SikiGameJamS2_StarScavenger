﻿using QFramework;
using UnityEngine;

namespace StarScavenger
{
    public  class Global: Architecture<Global>
    {
        protected override void Init()
        {
        }

        // 数据
        public static BindableProperty<int> HP = new(3);
        public static BindableProperty<int> MaxHP = new(3);
        public static BindableProperty<int> Fuel = new(100);
        public static BindableProperty<int> MaxFuel = new(100);

        public static BindableProperty<float> MoveSpeed = new(1f);
        public static BindableProperty<float> PropulsiveForceValue = new (1f);
        public static BindableProperty<float> Acceleration = new(0.5f);
        public static BindableProperty<float> RotateSpeed = new(0.5f);
        public static BindableProperty<int> FuelConsumption = new(1);

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // 设置 UI
            UIKit.Root.SetResolution(1920, 1080, 0.5f);
        }

        public static void ResetData()
        {
            HP.Value = MaxHP.Value;
            Fuel.Value = MaxFuel.Value;
            FuelConsumption.Value = 1;
        }
    }
}
