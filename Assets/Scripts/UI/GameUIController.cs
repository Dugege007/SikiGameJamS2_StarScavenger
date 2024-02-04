using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class GameUIController : ViewController
    {
        private void Awake()
        {
            Global.ResetData();
        }

        private void Start()
        {
            UIKit.OpenPanel<GamePanel>();
        }

        private void OnDestroy()
        {
            UIKit.ClosePanel<GamePanel>();
        }
    }
}
