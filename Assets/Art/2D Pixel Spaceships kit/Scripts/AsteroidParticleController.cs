using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidParticleController : MonoBehaviour
{
    public ParticleSystem AsteroidParticle;

    // Reproduce asteroid particle, deatach from perrent and destroy after play
    void Awake()
    {
        AsteroidParticle = GetComponent<ParticleSystem>();
        AsteroidParticle.Play();
        AsteroidParticle.transform.SetParent(null);
        Destroy(gameObject, AsteroidParticle.main.duration);
    }
}