using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public class Asteroid : MonoBehaviour
    {
        public float DefaultExistTime = 5f;
        private float mCurrentETime = 0;

        private Rigidbody2D rigid2D;

        private void Awake()
        {
            rigid2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Player player = Player.Default;
            if (player != null)
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= 12f)
                {
                    mCurrentETime = 0;
                    return;
                }
            }

            mCurrentETime += Time.deltaTime;

            if (mCurrentETime > DefaultExistTime)
            {
                this.DestroyGameObjGracefully();
            }
        }
    }
}
