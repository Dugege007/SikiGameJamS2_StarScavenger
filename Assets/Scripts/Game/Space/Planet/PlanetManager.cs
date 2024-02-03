using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Linq;

namespace StarScavenger
{
    public partial class PlanetManager : ViewController
    {
        public List<Planet> Planets = new List<Planet>();

        private void Update()
        {
            if (Time.frameCount % 60 == 0 && Player.Default != null)
            {
                Vector3 playerPosition = Player.Default.transform.position;

                // 查询到玩家距离最近的、未到达、未被发现的星球
                Planet nearestPlanet = Planets
                     .Where(planet => !planet.IsArrived && !planet.IsDiscover) // 添加过滤条件
                     .OrderBy(planet => (planet.transform.position - playerPosition).sqrMagnitude)
                     .FirstOrDefault();

                // 设置为下一个目标星球
                Player.Default.NextTargetPlanet = nearestPlanet;
            }
        }
    }
}
