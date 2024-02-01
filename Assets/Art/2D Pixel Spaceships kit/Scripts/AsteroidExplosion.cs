using QFramework;
using StarScavenger;
using System.Collections.Generic;
using UnityEngine;

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
                    .DestroySelfAfterDelayGracefully(1f);
            }

            Destroy(gameObject);

        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
}