using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacterScript : MonoBehaviour
{
    [SerializeField]
    private float _draggingMultiplyer = 500f;

    private List<ExtremityScript> _extremitiesList;

    private bool _isDragging = false;
    private GameObject _draggedExtremity;

    // Use this for initialization
    void Start()
    {
        _extremitiesList = new List<ExtremityScript>(this.transform.GetComponentsInChildren<ExtremityScript>());
    }

    void FixedUpdate()
    {
        //******************************************************
        // DEATH

        bool isDead = true;
        float deathPos = Camera.main.transform.position.y - Camera.main.orthographicSize - 1;

        foreach(ExtremityScript extremity in _extremitiesList)
        {
            if(extremity.transform.position.y >= deathPos)
            {
                isDead = false;
                break;
            }
        }

        if (isDead)
        {
            Debug.Log("Player Died!");

            Invoke("ReloadScene", 0.5f);
        }

        //******************************************************
        // MOVEMENT

        if (Input.GetMouseButton(0))
        {
            if (_isDragging)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                //_draggedExtremity.GetComponent<Rigidbody2D>().MovePosition(mousePos);
                _draggedExtremity.GetComponent<Rigidbody2D>().AddForce((mousePos - _draggedExtremity.transform.position) * _draggingMultiplyer);
            }
            else
            {
                Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                if (colliders != null)
                {
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.CompareTag("Extremity"))
                        {
                            int numOfAnchoredExtremities = 0;
                            foreach (ExtremityScript extremity in _extremitiesList)
                            {
                                if (extremity.IsAnchored)
                                {
                                    numOfAnchoredExtremities++;
                                }
                            }

                            if (numOfAnchoredExtremities > 1 || !collider.GetComponent<ExtremityScript>().IsAnchored)
                            {
                                _draggedExtremity = collider.gameObject;
                                //_draggedExtremity.GetComponent<Rigidbody2D>().isKinematic = true;
                                _draggedExtremity.GetComponent<HingeJoint2D>().enabled = false;

                                if (_draggedExtremity.GetComponent<HingeJoint2D>().connectedBody != null)
                                {
                                    _draggedExtremity.GetComponent<HingeJoint2D>().connectedBody.GetComponent<AnchorScript>().IsInUse = false;
                                    _draggedExtremity.GetComponent<HingeJoint2D>().connectedBody = null;
                                }

                                _isDragging = true;

                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (_isDragging)
            {
                //Collider2D[] colliders = Physics2D.OverlapPointAll(_draggedExtremity.transform.position);
                Collider2D[] colliders = Physics2D.OverlapBoxAll(_draggedExtremity.transform.position, _draggedExtremity.GetComponent<BoxCollider2D>().size, _draggedExtremity.transform.rotation.z);

                bool foundAnchor = false;

                if (colliders != null)
                {
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.CompareTag("Anchor") && collider.GetComponent<AnchorScript>().IsInUse == false)
                        {
                            collider.GetComponent<AnchorScript>().IsInUse = true;
                            _draggedExtremity.GetComponent<HingeJoint2D>().connectedBody = collider.GetComponent<Rigidbody2D>();

                            _draggedExtremity.GetComponent<HingeJoint2D>().enabled = true;

                            foundAnchor = true;

                            break;
                        }
                    }
                }

                if (!foundAnchor)
                {
                    _draggedExtremity.GetComponent<HingeJoint2D>().enabled = false;
                }

                //_draggedExtremity.GetComponent<Rigidbody2D>().isKinematic = false;
                _draggedExtremity = null;

                _isDragging = false;
            }
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
