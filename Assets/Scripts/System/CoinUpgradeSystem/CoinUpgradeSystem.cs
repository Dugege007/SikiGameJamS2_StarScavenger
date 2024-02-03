using QFramework;
using System.Collections.Generic;

namespace StarScavenger
{
    public class CoinUpgradeSystem : AbstractSystem, ICanSave
    {
        public static EasyEvent OnCoinUpgradeSystemChanged = new EasyEvent();

        public List<CoinUpgradeItem> Items { get; } = new List<CoinUpgradeItem>();

        protected override void OnInit()
        {
            // 最大生命值
            Add(new CoinUpgradeItem()
                .WithKey("max_hp_lv1")
                .WithDescription("初始生命值 + 1")
                .WithPrice(50)
                .OnUpgrade(item =>
                {
                    Global.HP.Value += 1;
                    Global.Coin.Value -= item.Price;
                }))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_hp_lv2")
                .WithDescription("初始生命值 + 1")
                .WithPrice(200)
                .OnUpgrade(item =>
                {
                    Global.HP.Value += 1;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_hp_lv3")
                .WithDescription("初始生命值 + 1")
                .WithPrice(1000)
                .OnUpgrade(item =>
                {
                    Global.HP.Value += 1;
                    Global.Coin.Value -= item.Price;
                })));

            // 燃料最大值增加
            Add(new CoinUpgradeItem()
                .WithKey("max_fuel_lv1")
                .WithDescription("燃料最大值 + 10")
                .WithPrice(10)
                .OnUpgrade(item =>
                {
                    Global.MaxFuel.Value += 10;
                    Global.Coin.Value -= item.Price;
                }))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_fuel_lv2")
                .WithDescription("燃料最大值 + 10")
                .WithPrice(20)
                .OnUpgrade(item =>
                {
                    Global.MaxFuel.Value += 10;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_fuel_lv3")
                .WithDescription("燃料最大值 + 10")
                .WithPrice(40)
                .OnUpgrade(item =>
                {
                    Global.MaxFuel.Value += 10;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_fuel_lv4")
                .WithDescription("燃料最大值 + 10")
                .WithPrice(100)
                .OnUpgrade(item =>
                {
                    Global.MaxFuel.Value += 10;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_fuel_lv5")
                .WithDescription("燃料最大值 + 10")
                .WithPrice(200)
                .OnUpgrade(item =>
                {
                    Global.MaxFuel.Value += 10;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_fuel_lv6")
                .WithDescription("燃料最大值 + 10")
                .WithPrice(500)
                .OnUpgrade(item =>
                {
                    Global.MaxFuel.Value += 10;
                    Global.Coin.Value -= item.Price;
                })));

            // 小行星获取资源增加
            Add(new CoinUpgradeItem()
                .WithKey("res_up_lv1")
                .WithDescription("获取资源小幅增加")
                .WithPrice(100)
                .OnUpgrade(item =>
                {
                    Global.MinCoinGet.Value++;
                    Global.MaxCoinGet.Value++;
                    Global.MinFuelGet.Value++;
                    Global.MaxFuelGet.Value++;
                    Global.Coin.Value -= item.Price;
                }))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("res_up_lv2")
                .WithDescription("获取资源小幅增加")
                .WithPrice(500)
                .OnUpgrade(item =>
                {
                    Global.MinCoinGet.Value++;
                    Global.MaxCoinGet.Value++;
                    Global.MinFuelGet.Value++;
                    Global.MaxFuelGet.Value++;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("res_up_lv3")
                .WithDescription("获取资源小幅增加")
                .WithPrice(1500)
                .OnUpgrade(item =>
                {
                    Global.MinCoinGet.Value++;
                    Global.MaxCoinGet.Value++;
                    Global.MinFuelGet.Value++;
                    Global.MaxFuelGet.Value++;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("res_up_lv4")
                .WithDescription("获取资源小幅增加")
                .WithPrice(3000)
                .OnUpgrade(item =>
                {
                    Global.MinCoinGet.Value++;
                    Global.MaxCoinGet.Value++;
                    Global.MinFuelGet.Value++;
                    Global.MaxFuelGet.Value++;
                    Global.Coin.Value -= item.Price;
                })))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("res_up_lv5")
                .WithDescription("获取资源小幅增加")
                .WithPrice(5000)
                .OnUpgrade(item =>
                {
                    Global.MinCoinGet.Value++;
                    Global.MaxCoinGet.Value++;
                    Global.MinFuelGet.Value++;
                    Global.MaxFuelGet.Value++;
                    Global.Coin.Value -= item.Price;
                })));

            // 速度上限
            Add(new CoinUpgradeItem()
                .WithKey("max_speed_lv1")
                .WithDescription("速度极限 + 2")
                .WithPrice(1500)
                .OnUpgrade(item =>
                {
                    Global.MaxSpeed.Value += 2f;
                    Global.Coin.Value -= item.Price;
                }))
                .Next(Add(new CoinUpgradeItem()
                .WithKey("max_speed_lv2")
                .WithDescription("速度极限 + 3")
                .WithPrice(5000)
                .OnUpgrade(item =>
                {
                    Global.MaxSpeed.Value += 3f;
                    Global.Coin.Value -= item.Price;
                })));

            Load();

            OnCoinUpgradeSystemChanged.Register(() =>
            {
                Save();
            });
        }

        public CoinUpgradeItem Add(CoinUpgradeItem item)
        {
            Items.Add(item);
            return item;
        }

        public void Save()
        {
            SaveSystem saveSystem = this.GetSystem<SaveSystem>();

            foreach (var coinUpgradeItem in Items)
            {
                saveSystem.SaveBool(coinUpgradeItem.Key, coinUpgradeItem.UpgradeFinish);
            }
        }

        public void Load()
        {
            SaveSystem saveSystem = this.GetSystem<SaveSystem>();

            foreach (var coinUpgradeItem in Items)
            {
                coinUpgradeItem.UpgradeFinish = saveSystem.LoadBool(coinUpgradeItem.Key, false);
            }
        }
    }
}
