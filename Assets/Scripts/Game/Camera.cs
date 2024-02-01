using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Camera : ViewController
    {
        public static Camera Default;

        private Vector2 mTargetPosition = Vector2.zero;

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void LateUpdate()
        {
            if (Player.Default)
            {
                mTargetPosition = Player.Default.transform.position;

                transform.position = new Vector3(mTargetPosition.x, mTargetPosition.y, transform.position.z);
                transform.rotation = Player.Default.transform.rotation;
            }
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
