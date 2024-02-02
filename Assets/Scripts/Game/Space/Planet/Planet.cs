using UnityEngine;
using QFramework;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace StarScavenger
{
    public partial class Planet : ViewController
    {
        public float MinRotationSpeed = 0.1f;
        public float MaxRotationSpeed = 1f;

        private float mRandomRotationSpeed;

        private void Start()
        {
            mRandomRotationSpeed = Random.Range(MinRotationSpeed, MaxRotationSpeed);
        }

        private void Update()
        {
            transform.Rotate(0, 0, mRandomRotationSpeed);
        }
    }
}
