using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //==========================================================================================
    // editable variables
    //==========================================================================================

    [SerializeField]
    private float _baseSpeed = 0.3f;

    [SerializeField]
    private float _maxHorizontalOffset = 2f;

    //==========================================================================================
    // other variable
    //==========================================================================================

    private Transform _playerTransform;
    private PlayerCharacterScript _playerScript;

    private int _stepIndex = -1;
    private float _acceleration;

    //==========================================================================================
    // monobehaviour methods
    //==========================================================================================

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

        float baseSpeed = _baseSpeed * Time.fixedDeltaTime;

        float acceleration = GameManagerScript.Instance.GetAccelerationStep((int)_playerScript.Altitude);

        if (acceleration > _acceleration)
        {
            _acceleration = acceleration;
            UIManager.Instance.Faster();
        }

        baseSpeed *= _acceleration;

        cameraTranslation.y += baseSpeed;

        //*****************************************
        // additional vertical movement (when the player is too near of the screen-top)

        float speedUpLimit = this.transform.position.y;
        float playerYPos = _playerTransform.position.y;

        if (playerYPos > speedUpLimit)
        {
            cameraTranslation.y += ((playerYPos - speedUpLimit) / Camera.main.orthographicSize) * _baseSpeed * 5 * Time.fixedDeltaTime;
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
