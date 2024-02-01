using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlink : MonoBehaviour
{
    private SpriteRenderer SpriteRendererColor;
    private Color color;
    private bool AlphaUp;

    void Start()
    {
        SpriteRendererColor = GetComponent<SpriteRenderer>();
        color = SpriteRendererColor.color;
        AlphaUp = true;
    }

    // Visual shield pulse (alpha channel)
    private void FixedUpdate()
    {
        color.a = AlphaUp ? 0.02f + color.a : color.a - 0.02f;

        if (color.a >= 0.5f)
        AlphaUp = false;
        else if (color.a <= 0.0f)
        AlphaUp = true;

    SpriteRendererColor.color = color;
    }

    // Sets alpha to maximum
    public void Blink()
    {
        color.a = (color.a = 1f);
    }
}