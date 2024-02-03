using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class GameUIController : ViewController
    {
        private void Start()
        {
            UIKit.OpenPanel<GamePanel>();
        }

        private void Update()
        {
            // 暂停游戏
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //TODO 打开暂停界面
                Time.timeScale = 0;
            }
        }

        private void OnDestroy()
        {
            UIKit.ClosePanel<GamePanel>();
        }
    }
}
