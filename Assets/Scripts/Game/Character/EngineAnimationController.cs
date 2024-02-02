using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarScavenger
{
    public class EngineAnimationController : MonoBehaviour
    {
        public Animator[] Engine;

        [Range(1, 4)]
        public int Preset;

        // Sets engine animation clip variant
        void Start()
        {
            Engine = GetComponentsInChildren<Animator>();

            for (int i = 0; i < Engine.Length; i++)
            {
                Engine[i].CrossFade("Engine_1_Small", 0.1f);
            }
        }
    }
}
