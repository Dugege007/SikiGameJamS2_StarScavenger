using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour
{
    public GameObject ParticleController;
    public List<GameObject> AsteroidPrefabs;

    // Create 3 random small asteroids and destroy big one
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Wakeup particle controller
        if (ParticleController != null) {ParticleController.SetActive(true);}

        for (int i = 0; i <= 2; i++)
        {
            int z = Random.Range(0,6);
            Instantiate(AsteroidPrefabs[z], transform.position + AsteroidPrefabs[z].transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}