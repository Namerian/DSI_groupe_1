using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremityScript : MonoBehaviour
{
    private bool _isMoving;
    private GameObject _helpCircle;

    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            if(!_isMoving && value)
            {
                _helpCircle = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/EmptyCircle"));
                _helpCircle.transform.parent = this.transform;
                _helpCircle.transform.position = this.transform.position;
            }
            else if(_isMoving && !value)
            {
                Destroy(_helpCircle);
                _helpCircle = null;
            }

            _isMoving = value;
        }
    }

    public bool IsAnchored { get; set; }

    // Use this for initialization
    void Start()
    {
        if (this.GetComponent<HingeJoint2D>().connectedBody != null)
        {
            IsAnchored = true;
        }
    }

    // Update is called once per frame
    /*void Update () {
		
	}*/
}
