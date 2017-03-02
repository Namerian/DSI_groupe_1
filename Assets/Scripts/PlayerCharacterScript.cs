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

    [SerializeField]
    private int _grabScoreBonus = 1000;

    //========================================================
    //
    //========================================================

    private List<ExtremityScript> _extremitiesList;

    private bool _isDragging = false;
    private bool _anchoringAllowed = false;
    private ExtremityScript _draggedExtremity;
    private float _levitationTimer;

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
            //**********************
            // mouse down AND dragging
            if (_isDragging)
            {
                int numOfAnchoredExtremities = 0;
                foreach (ExtremityScript extremity in _extremitiesList)
                {
                    if (extremity.IsAnchored)
                    {
                        numOfAnchoredExtremities++;
                    }
                }

                if (numOfAnchoredExtremities == 0)
                {
                    _levitationTimer += Time.fixedDeltaTime;
                }

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                _draggedExtremity.GetComponent<Rigidbody2D>().AddForce((mousePos - _draggedExtremity.transform.position) * _draggingMultiplyer);

                if (_levitationTimer > 1f)
                {
                    _draggedExtremity.IsMoving = false;
                    _draggedExtremity = null;

                    _isDragging = false;
                }
            }
            //**********************
            // mouse down AND NOT dragging
            else
            {
                Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                if (colliders != null)
                {
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.CompareTag("Extremity"))
                        {
                            ExtremityScript extremityScript = collider.GetComponent<ExtremityScript>();

                            _draggedExtremity = extremityScript;

                            _draggedExtremity.UnanchorExtremity();
                            _draggedExtremity.IsMoving = true;

                            _isDragging = true;
                            _anchoringAllowed = false;
                            _levitationTimer = 0f;

                            Invoke("AllowAnchoring", 0.3f);

                            break;
                        }
                    }
                }
            }
        }
        //**********************
        // mouse NOT down
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
                            if(!anchorScript.usedOnce)
                            {
                                anchorScript.usedOnce = true;
                                _uiManager.AddScore(_grabScoreBonus);
                            }

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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        GameManagerScript.Instance.LoadMenu((int)UIManager.Instance.score);
    }

    private void AllowAnchoring()
    {
        _anchoringAllowed = true;
    }
}
