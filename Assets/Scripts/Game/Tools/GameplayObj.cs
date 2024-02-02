using QFramework;
using UnityEngine;

namespace StarScavenger
{
    public abstract class GameplayObj : ViewController
    {
        public bool InScreen { get; set; }

        protected abstract Collider2D Collider2D { get; }

        private void OnBecameVisible()
        {
            //Debug.Log("打开碰撞器");
            Collider2D.enabled = true;
            InScreen = true;
        }

        private void OnBecameInvisible()
        {
            //Debug.Log("关闭碰撞器");
            Collider2D.enabled = false;
            InScreen = false;
        }
    }
}
