using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnchorScript : AnchorScript
{
    [SerializeField]
    private float _yOffset = 1f;
    [SerializeField]
    private float _speed = 1f;

    private Vector3 _basePos;
    private float _timer;

    AudioClip[] audioclips;

    // Use this for initialization
    void Start()
    {
        _basePos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime * _speed;

        //this.transform.position = new Vector3(_basePos.x, _basePos.y + Mathf.PingPong(_timer, _yOffset), _basePos.z);

        GetComponent<Rigidbody2D>().MovePosition(new Vector3(_basePos.x, _basePos.y + Mathf.PingPong(_timer, _yOffset), _basePos.z));
    }
}
