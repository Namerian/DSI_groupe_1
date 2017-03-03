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

    [SerializeField]
    private List<AmbianceBgListElement> _ambiancePrefabs;

    [SerializeField]
    private List<MaterialListElement> _crevasseMaterials;

    [SerializeField]
    private List<MaterialListElement> _anchorMaterials;

    [SerializeField]
    private List<int> _levelExperience;

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
            //Debug.LogError("GameManager has already been instantiated!");
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
            foreach (MaterialListElement element in _backgroundMaterials)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.material;
                }
            }

            Debug.LogError("Could not find background material for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public GameObject AmbianceBackground
    {
        get
        {
            foreach(AmbianceBgListElement element in _ambiancePrefabs)
            {
                if(element.environmentName == EnvironmentName)
                {
                    return element.ambiancePrefab;
                }
            }

            Debug.LogError("Could not find ambiance prefab for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public Material CrevasseMaterial
    {
        get
        {
            foreach (MaterialListElement element in _crevasseMaterials)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.material;
                }
            }

            Debug.LogError("Could not find crevasse material for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public Material AnchorMaterial
    {
        get
        {
            foreach (MaterialListElement element in _anchorMaterials)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.material;
                }
            }

            Debug.LogError("Could not find anchor material for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public int SessionScore { get; private set; }

    public int BestSessionScore { get; private set; }

    public int TotalScore { get; private set; }

    public int OldTotalScore { get { return TotalScore - SessionScore; } }

    public int MaxLevel { get { return _levelExperience.Count - 1; } }

    //==========================================================================================
    //
    //==========================================================================================

    public void StartGame()
    {
        SessionScore = 0;

        //***************************
        SceneManager.sceneLoaded += this.OnSceneLoaded;
        SceneManager.LoadSceneAsync("Scenes/TestLevel");
    }

    public void LoadMenu(int levelScore)
    {
        SessionScore = levelScore;

        if(levelScore > BestSessionScore)
        {
            BestSessionScore = levelScore;
        }

        TotalScore += levelScore;

        //***************************
        SceneManager.LoadSceneAsync("Scenes/Menu");
    }

    public int ComputeLevel(int experience)
    {
        int level = 0;

        for(int i = 0;i < _levelExperience.Count; i++)
        {
            if(experience > _levelExperience[i])
            {
                level++;
                experience -= _levelExperience[i];
            }
            else
            {
                break;
            }
        }

        return level;
    }

    //==========================================================================================
    //
    //==========================================================================================

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(this != _instance)
        {
            Debug.LogError("OnSceneLoaded called in wrong GameManager instance!");
            return;
        }

        if (scene.name == "TestLevel")
        {
            //Debug.Log("OnSceneLoaded: TestLevel called!");

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

        SceneManager.sceneLoaded -= this.OnSceneLoaded;
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

[System.Serializable]
public class AmbianceBgListElement
{
    public string environmentName;
    public GameObject ambiancePrefab;
}