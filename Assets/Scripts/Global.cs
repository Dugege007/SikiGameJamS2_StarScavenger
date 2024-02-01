using QFramework;
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
        public static BindableProperty<float> Fuel = new(100f);

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // 设置 UI
            UIKit.Root.SetResolution(1920, 1080, 0.5f);
        }

        public static void ResetData()
        {
            HP.Value = MaxHP.Value;
            Fuel.Value = 100f;
        }
    }
}
