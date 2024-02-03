using QFramework;
using System.Collections.Generic;
using UnityEngine;

namespace StarScavenger
{
    public class AsteroidExplosion : MonoBehaviour
    {
        public AsteroidType AsteroidType;
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
                if (AsteroidType == AsteroidType.Coin)
                {
                    int randomNum = Random.Range(1, 5);
                    Global.Coin.Value += randomNum;
                    FloatingTextController.Play("金币+" + randomNum, TextType.Coin);
                }
                else if (AsteroidType == AsteroidType.Fuel)
                {
                    int randomNum = Random.Range(2, 8);
                    Global.Fuel.Value += randomNum;
                    FloatingTextController.Play("燃料+" + randomNum, TextType.Fuel);
                }

                // Wakeup particle controller
                if (ParticleController != null) { ParticleController.SetActive(true); }

                for (int i = 0; i <= 2; i++)
                {
                    int z = Random.Range(0, 6);
                    AsteroidPrefabs[z].InstantiateWithParent(transform.parent)
                        .Position(transform.position + AsteroidPrefabs[z].transform.position)
                        .Rotation(Quaternion.identity)
                        .Show()
                        .DestroySelfAfterDelayGracefully(1f);
                }

                gameObject.DestroySelfGracefully();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
    }
}
