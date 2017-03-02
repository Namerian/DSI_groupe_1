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

    [SerializeField]
    private List<MaterialListElement> _backgroundMaterials;

    //==========================================================================================
    //
    //==========================================================================================

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.LogError("GameManager has already been instantiated!");
            Destroy(this.gameObject);
        }
    }

    //==========================================================================================
    //
    //==========================================================================================

    public int DifficultyLevel { get; set; }

    public string CharacterName { get; set; }

    public string EnvironmentName { get; set; }

    public Material BackgroundMaterial
    {
        get
        {
            foreach(MaterialListElement element in _backgroundMaterials)
            {
                if(element.environmentName == EnvironmentName)
                {
                    return element.material;
                }
            }

            Debug.LogError("Could not find material for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public int LevelScore { get; private set; }

    public int SessionScore { get; private set; }

    //==========================================================================================
    //
    //==========================================================================================

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/TestLevel");
    }

    public void LoadMenu(int levelScore)
    {
        LevelScore = levelScore;
        SessionScore += levelScore;

        SceneManager.LoadScene("Scenes/Menu");
    } 

    //==========================================================================================
    //
    //==========================================================================================

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == "TestLevel")
        {
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
            Rigidbody2D anchor1Rigidbody = startChunk.transform.Find("Anchors/Anchor_1").GetComponent<Rigidbody2D>();
            Rigidbody2D anchor2Rigidbody = startChunk.transform.Find("Anchors/Anchor_2").GetComponent<Rigidbody2D>();

            GameObject character = Instantiate<GameObject>(_characterPrefabs[charIndex].prefab);

            HingeJoint2D charLeftHand = GameObject.Find("Hand1").GetComponent<HingeJoint2D>();
            HingeJoint2D charRightHand = GameObject.Find("Hand2").GetComponent<HingeJoint2D>();

            float charYPos = anchor1Rigidbody.transform.position.y - (character.transform.position.y - charLeftHand.transform.position.y);

            character.transform.position = new Vector3(0, charYPos, 0);
            anchor1Rigidbody.transform.position = charLeftHand.transform.position;
            anchor2Rigidbody.transform.position = charRightHand.transform.position;

            charLeftHand.connectedBody = anchor1Rigidbody;
            charRightHand.connectedBody = anchor2Rigidbody;
        }
        else if(scene.name == "Menu")
        {

        }
        
    }
}

[System.Serializable]
public class CharacterListElement
{
    public string charName;
    public GameObject prefab;
}

[System.Serializable]
public class MaterialListElement
{
    public string environmentName;
    public Material material;
}