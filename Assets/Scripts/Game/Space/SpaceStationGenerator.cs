using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace StarScavenger
{
    public partial class SpaceStationGenerator : ViewController
    {
        public List<SpaceStation> SpaceStations = new List<SpaceStation>();

        private void Start()
        {
            foreach (var spaceStation in SpaceStations)
                spaceStation.Hide();

        }


    }
}
