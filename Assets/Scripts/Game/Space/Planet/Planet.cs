using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class Planet : ViewController
    {
        public float MinRotationSpeed = 0.1f;
        public float MaxRotationSpeed = 1f;
        // 星球表面的重力加速度
        public float Gravity = 10f;
        public float Radius = 1.5f;
        private float mRandomRotationSpeed;

        private void Start()
        {
            Radius = HitHurtBox.radius;

            mRandomRotationSpeed = Random.Range(MinRotationSpeed, MaxRotationSpeed);

            GravityArea.OnTriggerStay2DEvent(collider2D =>
            {
                if (collider2D.GetComponentInParent<Player>())
                    Player.Default.GravityEffect(this);

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            GravityArea.OnTriggerExit2DEvent(collider2D =>
            {
                if (collider2D.GetComponentInParent<Player>())
                    Player.Default.GravityEffect(null);

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            transform.Rotate(0, 0, mRandomRotationSpeed);
        }
    }
}
