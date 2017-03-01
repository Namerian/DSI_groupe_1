using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //========================================================
    //
    //========================================================

    [SerializeField]
    private float _scrollSpeed = 1f;

    [SerializeField]
    private float _maxHorizontalOffset = 2f;

    //========================================================
    //
    //========================================================

    private Transform _playerTransform;

    //========================================================
    //
    //========================================================

    // Use this for initialization
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("body");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 cameraTranslation = Vector3.zero;

        //*****************************************
        // normal vertical movement

        cameraTranslation.y += _scrollSpeed * Time.fixedDeltaTime;

        //*****************************************
        // additional vertical movement (when the player is too near of the screen-top)

        float speedUpLimit = this.transform.position.y;
        float playerYPos = _playerTransform.position.y;

        if(playerYPos > speedUpLimit)
        {
            cameraTranslation.y += ((playerYPos - speedUpLimit) / Camera.main.orthographicSize) * _scrollSpeed * 5 * Time.fixedDeltaTime;
        }

        //*****************************************
        // horizontal movement

        float targetX = _playerTransform.position.x;
        float cameraX = this.transform.position.x;

        if (targetX > _maxHorizontalOffset)
        {
            targetX = _maxHorizontalOffset;
        }
        else if (targetX < -_maxHorizontalOffset)
        {
            targetX = -_maxHorizontalOffset;
        }

        if (targetX != cameraX)
        {
            cameraTranslation.x = (targetX - cameraX) * Time.deltaTime;
        }

        this.transform.Translate(cameraTranslation);
    }
}
