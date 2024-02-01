using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    ShieldBlink ShieldScript;
    public bool ShieldIsActive;

    // Get reference to ShieldBlink script in child gameobject, check shield (enable/disable)
    void Start()
    {
        ShieldScript = gameObject.GetComponentInChildren<ShieldBlink>();
        if (ShieldIsActive) {
            ShieldScript.gameObject.SetActive(true);
        }
        else ShieldScript.gameObject.SetActive(false);
    }

    // Use blink function in ShieldBlink script
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShieldScript)
        {
            ShieldScript.Blink();
        }
    }
}
