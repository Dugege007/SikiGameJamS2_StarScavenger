using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAnimationPreset : MonoBehaviour
{
    public Animator[] Engine;

    [Range(1,4)]
    public int Preset;

    // Sets engine animation clip variant
    void Start()
    {
        Engine = GetComponentsInChildren<Animator>();

        for (int i = 0; i < Engine.Length; i++)
        {
            Engine[i].SetInteger("ColorPreset", Preset);
        }
    }
}
