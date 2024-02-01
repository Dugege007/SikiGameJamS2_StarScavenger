using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public float disappearTime = 5f;
    public AnimationClip ExplosionClip;
    Animator m_Animator;

    void Start()
    {
        // Destroy object if it didn't hit anything
        Destroy(gameObject, disappearTime);
    }

    // Play explosion animation and destoy projectile
    void OnCollisionEnter2D(Collision2D collision)
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Animator.SetTrigger("Hit");
        Destroy(gameObject, ExplosionClip.length);
    }
}