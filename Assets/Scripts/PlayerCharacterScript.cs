using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacterScript : MonoBehaviour
{
    //========================================================
    //
    //========================================================

    [SerializeField]
    private float _draggingMultiplyer = 500f;

    [SerializeField]
    private float _anchorBreakForce = 100f;

    //========================================================
    //
    //========================================================

    private List<ExtremityScript> _extremitiesList;

    private bool _isDragging = false;
    private bool _anchoringAllowed = false;
    private ExtremityScript _draggedExtremity;

    private UIManager _uiManager;
    private int _numOfAnchoredExtremitiesTest;

    private Transform _body;
    private float _originalYpos;
    private float _altitude;

    //========================================================
    //
    //========================================================

    public float Altitude { get { return _altitude; } }

    // Use this for initialization
    void Start()
    {
        _extremitiesList = new List<ExtremityScript>(this.transform.GetComponentsInChildren<ExtremityScript>());
        //Debug.Log("Player has " + _extremitiesList.Count + " extremities!");
        _uiManager = FindObjectOfType<UIManager>();
        _body = transform.Find("body");
        _originalYpos = _body.position.y;
    }

    //========================================================
    //
    //========================================================

    void Update()
    {
        _numOfAnchoredExtremitiesTest = 0;
        foreach (ExtremityScript extremity in _extremitiesList)
        {
            if (extremity.IsAnchored)
            {
                _numOfAnchoredExtremitiesTest++;
            }
        }

        _uiManager.UpdateCombo(_numOfAnchoredExtremitiesTest);
        //Debug.Log(numOfAnchoredExtremitiesTest);

        _altitude = _body.position.y - _originalYpos;
        _uiManager.UpdateAltitude(_altitude);
    }

    //========================================================
    //
    //========================================================

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
                //Debug.Log("dragging");

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

                            ExtremityScript extremityScript = collider.GetComponent<ExtremityScript>();

                            if (numOfAnchoredExtremities > 1 || !extremityScript.IsAnchored)
                            {
                                _draggedExtremity = extremityScript;

                                _draggedExtremity.UnanchorExtremity();
                                _draggedExtremity.IsMoving = true;

                                _isDragging = true;
                                _anchoringAllowed = false;
                                Invoke("AllowAnchoring", 0.3f);

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
            if (_isDragging && _anchoringAllowed)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(_draggedExtremity.transform.position, _draggedExtremity.GetComponent<BoxCollider2D>().size, _draggedExtremity.transform.rotation.z);

                if (colliders != null)
                {
                    foreach (Collider2D collider in colliders)
                    {
                        AbstractAnchorScript anchorScript = collider.GetComponent<AbstractAnchorScript>();

                        if (collider.CompareTag("Anchor") && anchorScript.IsInUse == false)
                        {
                            //Debug.Log("end of drag, anchored extremity");

                            _draggedExtremity.AnchorExtremity(anchorScript, _anchorBreakForce);
                            _uiManager.AddScore(1000);

                            break;
                        }
                    }
                }

                _draggedExtremity.IsMoving = false;
                _draggedExtremity = null;

                _isDragging = false;
            }
            else if (_isDragging)
            {
                _draggedExtremity.IsMoving = false;
                _draggedExtremity = null;

                _isDragging = false;
            }
        }
    }

    //========================================================
    //
    //========================================================

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void AllowAnchoring()
    {
        _anchoringAllowed = true;
    }
}
