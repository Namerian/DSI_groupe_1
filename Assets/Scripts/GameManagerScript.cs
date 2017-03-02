using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //==========================================================================================
    //
    //==========================================================================================

    private static GameManagerScript _instance;

    public static GameManagerScript Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is not instantiated!");
            }

            return _instance;
        }
    }

    //==========================================================================================
    //
    //==========================================================================================

    [SerializeField]
    private List<CharacterListElement> _characterPrefabs;

    //==========================================================================================
    //
    //==========================================================================================

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogError("GameManager has already been instantiated!");
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    /*void Update()
    {

    }*/

    //==========================================================================================
    //
    //==========================================================================================

    public int DifficultyLevel { get; set; }

    public string CharacterName { get; set; }

    //==========================================================================================
    //
    //==========================================================================================

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/TestLevel");

        int charIndex = 0;

        for (int i = 0; i < _characterPrefabs.Count; i++)
        {
            CharacterListElement element = _characterPrefabs[i];

            if (CharacterName == element.charName)
            {
                charIndex = i;
            }
        }

        GameObject startChunk = GameObject.Find("StartChunk");
        Rigidbody2D anchor1Rigidbody = startChunk.transform.Find("Anchor_1").GetComponent<Rigidbody2D>();
        Rigidbody2D anchor2Rigidbody = startChunk.transform.Find("Anchor_2").GetComponent<Rigidbody2D>();

        GameObject character = Instantiate<GameObject>(_characterPrefabs[charIndex].prefab);
        HingeJoint2D charLeftHand = character.transform.Find("Hand1").GetComponent<HingeJoint2D>();
        HingeJoint2D charRightHand = character.transform.Find("Hand2").GetComponent<HingeJoint2D>();

        float charYPos = anchor1Rigidbody.transform.position.y - (character.transform.position.y - charLeftHand.transform.position.y);

        character.transform.position = new Vector3(0, charYPos, 0);
        anchor1Rigidbody.transform.position = charLeftHand.transform.position;
        anchor2Rigidbody.transform.position = charRightHand.transform.position;

        charLeftHand.connectedBody = anchor1Rigidbody;
        charRightHand.connectedBody = anchor2Rigidbody;
    }
}

[System.Serializable]
public class CharacterListElement
{
    public string charName;
    public GameObject prefab;
}