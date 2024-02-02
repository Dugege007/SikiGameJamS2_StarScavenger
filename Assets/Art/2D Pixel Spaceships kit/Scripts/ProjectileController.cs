using UnityEngine;

namespace StarScavenger
{
    public class ProjectileController : MonoBehaviour
    {
        public float disappearTime = 5f;
        public AnimationClip ExplosionClip;
        private Animator m_Animator;
        private Rigidbody2D m_Rigidbody2D;

        void Start()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

            // Destroy object if it didn't hit anything
            Destroy(gameObject, disappearTime);
        }

        private void FixedUpdate()
        {
            m_Rigidbody2D.velocity = transform.up *  Global.ProjectileSpeed.Value;
        }

        // Play explosion animation and destoy projectile
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponentInParent<Asteroid>())
            {
                m_Animator = gameObject.GetComponent<Animator>();
                m_Animator.SetTrigger("Hit");
                Destroy(gameObject, ExplosionClip.length);
            }
        }
    }
}
