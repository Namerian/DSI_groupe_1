﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnchorScript : AnchorScript
{
    [SerializeField]
    private float _yOffset = 1f;
    [SerializeField]
    private float _xOffset = 1f;
    [SerializeField]
    private float _speed = 1f;

    private Vector3 _basePos;
    private float _timer;

    AudioClip[] audioclips;

    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        _basePos = this.transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime * _speed;

        //this.transform.position = new Vector3(_basePos.x, _basePos.y + Mathf.PingPong(_timer, _yOffset), _basePos.z);

        if(_yOffset != 0 && _xOffset != 0) rb.MovePosition(new Vector3(_basePos.x + Mathf.PingPong(_timer, _xOffset), _basePos.y + Mathf.PingPong(_timer, _yOffset), _basePos.z));
        else if (_yOffset == 0) rb.MovePosition(new Vector3(_basePos.x + Mathf.PingPong(_timer, _xOffset), _basePos.y, _basePos.z));
        else if (_xOffset == 0) rb.MovePosition(new Vector3(_basePos.x, _basePos.y + Mathf.PingPong(_timer, _yOffset), _basePos.z));
    }
}
