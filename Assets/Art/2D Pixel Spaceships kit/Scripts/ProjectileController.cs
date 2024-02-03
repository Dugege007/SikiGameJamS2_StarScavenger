using UnityEngine;

namespace StarScavenger
{
    public class ProjectileController : MonoBehaviour
    {
        public GameObject Owner;
        public float disappearTime = 5f;
        public AnimationClip ExplosionClip;
        public Animator Animator;
        private Rigidbody2D m_Rigidbody2D;

        private void Start()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

            Destroy(gameObject, disappearTime);
        }

        private void FixedUpdate()
        {
            m_Rigidbody2D.velocity = transform.up * Global.ProjectileSpeed.Value;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HitHurtBox hitHurtBox = collision.gameObject.GetComponentInChildren<HitHurtBox>();

            if (hitHurtBox != null)
            {
                if (hitHurtBox.Owner.CompareTag("Asteroid"))
                {
                    Animator = gameObject.GetComponentInChildren<Animator>();
                    Animator.CrossFade("Projectile_3_Explosion", 0.1f);

                    if (hitHurtBox.Owner.GetComponent<Asteroid>().AsteroidType == AsteroidType.Coin)
                    {
                        int randomNum = Random.Range(1, 5);
                        Global.Coin.Value += randomNum;
                        FloatingTextController.Play("金币+" + randomNum, TextType.Coin);
                    }
                    else if (hitHurtBox.Owner.GetComponent<Asteroid>().AsteroidType == AsteroidType.Fuel)
                    {
                        int randomNum = Random.Range(2, 8);
                        Global.Fuel.Value += randomNum;
                        FloatingTextController.Play("燃料+" + randomNum, TextType.Fuel);
                    }

                    Destroy(gameObject, ExplosionClip.length);
                }

                if (hitHurtBox.Owner.CompareTag("Player"))
                {
                    Animator = gameObject.GetComponentInChildren<Animator>();
                    Animator.CrossFade("Projectile_3_Explosion", 0.1f);
                    if (Global.Shield.Value > 0)
                        Global.Shield.Value--;
                    else
                        Global.HP.Value--;

                    Destroy(gameObject, ExplosionClip.length);
                }
            }
        }
    }
}
