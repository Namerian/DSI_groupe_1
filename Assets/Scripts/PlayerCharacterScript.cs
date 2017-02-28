using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacterScript : MonoBehaviour
{
    [SerializeField]
    private float _draggingMultiplyer = 500f;

    [SerializeField]
    private float _anchorBreakForce = 100f;

    private List<ExtremityScript> _extremitiesList;

    private bool _isDragging = false;
    private ExtremityScript _draggedExtremity;

    // Use this for initialization
    void Start()
    {
        _extremitiesList = new List<ExtremityScript>(this.transform.GetComponentsInChildren<ExtremityScript>());
        Debug.Log("Player has " + _extremitiesList.Count + " extremities!");
    }

    void FixedUpdate()
    {
        //******************************************************
        // DEATH

        bool isDead = true;
        float deathPos = Camera.main.transform.position.y - Camera.main.orthographicSize - 1;

        foreach (ExtremityScript extremity in _extremitiesList)
        {
            if (extremity.transform.position.y >= deathPos)
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
                Debug.Log("dragging");

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

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

                            Debug.Log(numOfAnchoredExtremities);
                            ExtremityScript extremityScript = collider.GetComponent<ExtremityScript>();

                            if (numOfAnchoredExtremities > 1 || !extremityScript.IsAnchored)
                            {
                                _draggedExtremity = extremityScript;

                                _draggedExtremity.UnanchorExtremity();
                                _draggedExtremity.IsMoving = true;
                                _isDragging = true;
                                break;
                            }

                       //Plutôt que le if qu'il y a juste au dessus, il faudrait faire un truc qui vérifie si le joueur n'est accroché nulle part au moment où il commence le drag
                       //Et si c'est le cas, annuler son drag pour le faire tomber. J'ai essayé de le faire en dessous mais j'ai pas réussi
                            /*
                            if (numOfAnchoredExtremities <= 1)
                            {
                                _draggedExtremity.IsMoving = false;
                                _isDragging = false;
                            }
                            else
                            {
                                _isDragging = true;
                            }                               
                            */
                        }
                    }
                }
            }
        }
        else
        {
            if (_isDragging)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(_draggedExtremity.transform.position, _draggedExtremity.GetComponent<BoxCollider2D>().size, _draggedExtremity.transform.rotation.z);

                if (colliders != null)
                {
                    foreach (Collider2D collider in colliders)
                    {
                        AnchorScript anchorScript = collider.GetComponent<AnchorScript>();

                        if (collider.CompareTag("Anchor") && anchorScript.IsInUse == false)
                        {
                            Debug.Log("end of drag, anchored extremity");

                            _draggedExtremity.AnchorExtremity(anchorScript, _anchorBreakForce); //Dans la fonction AnchorExtremity, le _anchorbreakforce ne s'appliquera que si c'est un Moving Anchor

                            break;
                        }
                    }
                }

                _draggedExtremity.IsMoving = false;
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
