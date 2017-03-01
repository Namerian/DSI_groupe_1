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

    [SerializeField]
    private List<Vector2> _accelerationSteps;

    //========================================================
    //
    //========================================================

    private Transform _playerTransform;
    private PlayerCharacterScript _playerScript;

    //========================================================
    //
    //========================================================

    // Use this for initialization
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("body");
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacterScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 cameraTranslation = Vector3.zero;

        //*****************************************
        // normal vertical movement

        float baseSpeed = _scrollSpeed * Time.fixedDeltaTime;

        for (int i = _accelerationSteps.Count; i-- > 0;)
        {
            if (_playerScript.Altitude >= _accelerationSteps[i].x)
            {
                baseSpeed *= _accelerationSteps[i].y;
                break;
            }
        }

        cameraTranslation.y += baseSpeed;

        //*****************************************
        // additional vertical movement (when the player is too near of the screen-top)

        float speedUpLimit = this.transform.position.y;
        float playerYPos = _playerTransform.position.y;

        if (playerYPos > speedUpLimit)
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
