using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremityScript : MonoBehaviour
{
    private HingeJoint2D _hingeJoint;

    private bool _isMoving;
    private GameObject _helpCircle;
    private AnchorScript _anchor;

    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            if (!_isMoving && value)
            {
                _helpCircle = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/EmptyCircle"));
                _helpCircle.transform.parent = this.transform;
                _helpCircle.transform.position = this.transform.position;
            }
            else if (_isMoving && !value)
            {
                Destroy(_helpCircle);
                _helpCircle = null;
            }

            _isMoving = value;
        }
    }

    public bool IsAnchored { get; private set; }

    // Use this for initialization
    void Start()
    {
        _hingeJoint = this.GetComponent<HingeJoint2D>();

        if (_hingeJoint.connectedBody != null)
        {
            _hingeJoint.enabled = true;
            IsAnchored = true;
        }
        else
        {
            _hingeJoint.enabled = false;
            IsAnchored = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_hingeJoint != null)
        {
            if (_hingeJoint.connectedBody != null && IsAnchored == false)
            {
                _hingeJoint.enabled = true;
                IsAnchored = true;
            }
            else if (_hingeJoint.connectedBody == null && IsAnchored == true)
            {
                _hingeJoint.enabled = false;
                IsAnchored = false;
            }
        }
    }

    public void AnchorExtremity(AnchorScript anchor, float breakForce)
    {
        _anchor = anchor;

        if (_hingeJoint == null)
        {
            _hingeJoint = this.gameObject.AddComponent<HingeJoint2D>();
        }

        _hingeJoint.connectedBody = anchor.GetComponent<Rigidbody2D>();
        if (anchor.GetComponent<MovingAnchorScript>()) _hingeJoint.breakForce = breakForce;  //On change la breakforce uniquement si l'anchor est un moving anchor
        else if (_hingeJoint.breakForce != Mathf.Infinity) _hingeJoint.breakForce = 99999;  //Faire _hingJoint.breakForce = Mathf.Infinity
        _hingeJoint.enabled = true;

        _anchor.IsInUse = true;

        IsAnchored = true;
    }

    public void UnanchorExtremity()
    {
        if (_anchor != null)
        {
            _anchor.IsInUse = false;
        }

        if (_hingeJoint != null)
        {
            _hingeJoint.connectedBody = null;
            _hingeJoint.enabled = false;
        }

        IsAnchored = false;
    }
}
