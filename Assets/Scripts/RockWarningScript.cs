using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWarningScript : MonoBehaviour
{
    //private Transform _arrowTransform;
    private Transform _rock;

    // Use this for initialization
    void Start()
    {
        //_arrowTransform = this.transform.Find("Arrow");

        //_arrowTransform.up = (_rock.position - this.transform.position).normalized;

        float yPos = Camera.main.transform.position.y + Camera.main.orthographicSize - 0.5f;

        float halfScreenWidth = ((Camera.main.orthographicSize * 2) / Camera.main.pixelHeight) * Camera.main.pixelWidth * 0.5f;
        float leftLimit = Camera.main.transform.position.x - halfScreenWidth + 0.5f;
        float rightLimit = Camera.main.transform.position.x + halfScreenWidth - 0.5f;

        float xPos = Mathf.Clamp(_rock.transform.position.x, leftLimit, rightLimit);

        this.transform.position = new Vector3(_rock.transform.position.x, yPos, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (_rock == null || _rock.position.y <= this.transform.position.y)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            Destroy(this.gameObject);

            return;
        }

        //_arrowTransform.up = (_rock.position - this.transform.position).normalized;

        float yPos = Camera.main.transform.position.y + Camera.main.orthographicSize - 0.5f;

        float halfScreenWidth = ((Camera.main.orthographicSize * 2) / Camera.main.pixelHeight) * Camera.main.pixelWidth * 0.5f;
        float leftLimit = Camera.main.transform.position.x - halfScreenWidth + 0.5f;
        float rightLimit = Camera.main.transform.position.x + halfScreenWidth - 0.5f;

        float xPos = Mathf.Clamp(_rock.transform.position.x, leftLimit, rightLimit);

        this.transform.position = new Vector3(_rock.transform.position.x, yPos, -1);
    }

    public void Initialize(Transform rock)
    {
        _rock = rock;
    }
}
