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
            // ��ͣ��Ϸ
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //TODO ����ͣ����
                Time.timeScale = 0;
            }
        }

        private void OnDestroy()
        {
            UIKit.ClosePanel<GamePanel>();
        }
    }
}
