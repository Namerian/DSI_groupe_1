using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAnchorScript : AnchorScript
{
    [SerializeField]
    private float _maxUseTime = 2f;

    private float _useTimer = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsInUse)
        {
            if(_useTimer > _maxUseTime)
            {
                Destroy(this.gameObject);
            }

            _useTimer += Time.deltaTime;
        }
    }
}
