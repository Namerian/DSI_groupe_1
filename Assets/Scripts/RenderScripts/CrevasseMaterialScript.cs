using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrevasseMaterialScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Material material = GameManagerScript.Instance.CrevasseMaterial;

        if (material != null)
        {
            renderer.material = material;
        }
    }
}
