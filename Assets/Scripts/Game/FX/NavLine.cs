using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class NavLine : ViewController
    {
        private void Start()
        {
            Animator animator = GetComponent<Animator>();

            Global.IsAboutCollide.RegisterWithInitValue(isAboutCollide =>
            {
                if (isAboutCollide)
                    animator.CrossFade("BlinkRed", 0.1f);
                else
                    animator.CrossFade("BlinkWhite", 0.1f);

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
    }
}
