using QFramework;
using System.Collections.Generic;
using UnityEngine;

namespace StarScavenger
{
    public class AsteroidExplosion : MonoBehaviour
    {
        public GameObject ParticleController;
        public List<GameObject> AsteroidPrefabs;
        private HitHurtBox mHitHurtBox;

        private void Awake()
        {
            mHitHurtBox = GetComponentInChildren<HitHurtBox>();
        }

        private void Start()
        {
            // Create 3 random small asteroids and destroy big one
            mHitHurtBox.OnCollisionEnter2DEvent(collision =>
            {
                // Wakeup particle controller
                if (ParticleController != null) { ParticleController.SetActive(true); }

                for (int i = 0; i <= 2; i++)
                {
                    int z = Random.Range(0, 6);
                    AsteroidPrefabs[z].InstantiateWithParent(transform.parent)
                        .Position(transform.position + AsteroidPrefabs[z].transform.position)
                        .Rotation(Quaternion.identity)
                        .Show()
                        .Self(self =>
                        {
                            self.GetComponent<Rigidbody2D>().AddForce(RandomUtility.Choose(Vector2.up, Vector2.left, Vector2.right, Vector2.down), ForceMode2D.Impulse);
                        })
                        .DestroySelfAfterDelayGracefully(1f);
                }

                gameObject.DestroySelfGracefully();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
    }
}
