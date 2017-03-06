using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColourScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color colour = GameManagerScript.Instance.BackgroundColour;

        renderer.color = colour;
    }
}
