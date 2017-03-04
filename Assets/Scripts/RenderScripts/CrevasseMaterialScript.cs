using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrevasseMaterialScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = GameManagerScript.Instance.CrevasseColor;

        if (color != null)
        {
            renderer.color = color;
        }
    }
}
