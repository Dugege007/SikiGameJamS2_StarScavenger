using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarScavenger
{
    public enum StationType
    {
        None,
        HP,
        Fuel,
        WeaponUpgrade,
        ShieldRepair,
        ResourceTrade,
        NavCenter,
    }

    public class SpaceStation : MonoBehaviour
    {
        public StationType StationType;


    }
}
