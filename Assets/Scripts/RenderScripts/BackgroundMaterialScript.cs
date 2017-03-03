using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMaterialScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Material material = GameManagerScript.Instance.BackgroundMaterial;

        if (material != null)
        {
            renderer.material = material;
        }
    }
}
