using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarScavenger
{
    public class CoinUpgradeItem
    {
        public EasyEvent OnChanged = new EasyEvent();

        private CoinUpgradeItem mNext = null;

        public bool UpgradeFinish { get; set; } = false;
        public string Key { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }

        private Action<CoinUpgradeItem> mOnUpgrade;
        private Func<CoinUpgradeItem, bool> mCondition;

        public void Upgrade()
        {
            mOnUpgrade?.Invoke(this);
            UpgradeFinish = true;
            TriggerOnChanged();
            CoinUpgradeSystem.OnCoinUpgradeSystemChanged.Trigger(); // 触发一下事件
        }

        public void TriggerOnChanged()
        {
            OnChanged.Trigger();
            mNext?.TriggerOnChanged();
        }

        /// <summary>
        /// 条件检测
        /// </summary>
        /// <returns></returns>
        public bool ConditionCheck()
        {
            if (mCondition != null)
                return !UpgradeFinish && mCondition.Invoke(this);

            return !UpgradeFinish;
        }

        public CoinUpgradeItem WithKey(string key)
        {
            Key = key;
            return this;
        }

        public CoinUpgradeItem WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public CoinUpgradeItem WithPrice(int price)
        {
            Price = price;
            return this;
        }

        public CoinUpgradeItem OnUpgrade(Action<CoinUpgradeItem> onUpgrade)
        {
            mOnUpgrade = onUpgrade;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public CoinUpgradeItem Condition(Func<CoinUpgradeItem, bool> condition)
        {
            mCondition = condition;
            return this;
        }

        public CoinUpgradeItem Next(CoinUpgradeItem next)
        {
            mNext = next;
            mNext.Condition(_ => UpgradeFinish);
            return mNext;
        }
    }
}
