using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class GameStartController : ViewController
    {
        private void Start()
        {
            UIKit.OpenPanel<GameStartPanel>();
        }
    }
}
