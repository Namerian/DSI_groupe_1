using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeed = 1f;

    [SerializeField]
    private float _maxHorizontalOffset = 2f;

    private Transform _playerTransform;

    // Use this for initialization
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("body");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 verticalTranslation = Vector3.up * _scrollSpeed * Time.fixedDeltaTime;
        Vector3 horizontalTranslation = Vector3.zero;

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
            horizontalTranslation.x = (targetX - cameraX) * Time.deltaTime;
        }

        this.transform.Translate(verticalTranslation + horizontalTranslation);
    }
}
