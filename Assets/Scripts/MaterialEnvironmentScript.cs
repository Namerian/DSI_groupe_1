using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialEnvironmentScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material = GameManagerScript.Instance.BackgroundMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
