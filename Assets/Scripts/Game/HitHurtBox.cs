using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class HitHurtBox : GameplayObj
    {
        public GameObject Owner;
        private Collider2D mCollider2D;

        protected override Collider2D Collider2D => mCollider2D;

        private void Awake()
        {
            mCollider2D = GetComponent<Collider2D>();
        }

        private void Start()
        {
            if (Owner == null)
            {
                Owner = transform.parent.gameObject;
            }
        }
    }
}
