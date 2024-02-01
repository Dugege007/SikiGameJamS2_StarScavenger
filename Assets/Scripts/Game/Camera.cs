using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Camera : ViewController
    {
        private Vector2 mTargetPosition = Vector2.zero;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void LateUpdate()
        {
            if (PlayerSpaceShip.Default)
            {
                mTargetPosition = PlayerSpaceShip.Default.transform.position;

                transform.position = new Vector3(mTargetPosition.x, mTargetPosition.y, transform.position.z);
                transform.rotation = PlayerSpaceShip.Default.transform.rotation;
            }
        }
    }
}
