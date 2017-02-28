using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremityScript : MonoBehaviour
{
    public bool IsAnchored { get; set; }

    // Use this for initialization
    void Start () {
		if(this.GetComponent<HingeJoint2D>().connectedBody != null)
        {
            IsAnchored = true;
        }
	}

    // Update is called once per frame
    /*void Update () {
		
	}*/
}
