using UnityEngine;
using QFramework;

namespace StarScavenger
{
    public partial class SFXManager : ViewController
    {
        public static SFXManager Default;

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
