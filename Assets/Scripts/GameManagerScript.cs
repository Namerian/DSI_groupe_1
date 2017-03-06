﻿using System.Collections;
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
    private List<ColourListElement> _backgroundColours;

    [SerializeField]
    private List<AmbianceBgListElement> _ambiancePrefabs;

    [SerializeField]
    private List<ColourListElement> _crevasseColours;

    [SerializeField]
    private List<ColourListElement> _wallColours;

    [SerializeField]
    private List<ColourListElement> _wallShadowColours;

    [SerializeField]
    private List<MaterialListElement> _anchorMaterials;

    [SerializeField]
    private List<SpriteListElement> _plantSprites;

    [SerializeField]
    private List<SpriteListElement> _UIImages;

    [SerializeField]
    private List<int> _levelExperience;

    [SerializeField]
    private List<float> _scoreMultipliers;

    //==========================================================================================
    //
    //==========================================================================================

    //==========================================================================================
    //
    //==========================================================================================

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            SceneManager.sceneLoaded += this.OnSceneLoaded;

            DontDestroyOnLoad(this.gameObject);

            TotalScore = PlayerPrefs.GetInt("TotalScore");
            BestSessionScore = PlayerPrefs.GetInt("BestSessionScore");

            AmplitudeHelper.AppId = "e42975312282ef47be31ec6af5cb48fc";
            AmplitudeHelper.Instance.FillCustomProperties += FillTrackingProperties;
            //AmplitudeHelper.Instance.StartSession();
            AmplitudeHelper.Instance.LogEvent("Start Game");
        }
        else
        {
            //Debug.LogError("GameManager has already been instantiated!");
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if(_instance == this)
        {
            PlayerPrefs.SetInt("TotalScore", TotalScore);
            PlayerPrefs.SetInt("BestSessionScore", BestSessionScore);

            /*int timeInSeconds = (int)Time.time;
            int seconds = timeInSeconds % 60;
            int minutes = timeInSeconds % 3600;

            Debug.Log("timeInSeconds: " + Time.time);
            Debug.Log("timeInSeconds: " + Time.realtimeSinceStartup);
            Debug.Log("time: " + string.Format("{0:00}:{1:00}", minutes, seconds));*/

            //Amplitude.Instance.logEvent("Exit Game");
            //Amplitude.Instance.endSession();

            AmplitudeHelper.Instance.LogEvent("Exit Game");
            //AmplitudeHelper.Instance.EndSession();
        }
    }

    //==========================================================================================
    //
    //==========================================================================================

    public int DifficultyLevel { get; set; }

    public string CharacterName { get; set; }

    public string EnvironmentName { get; set; }

    public Color BackgroundColour
    {
        get
        {
            foreach (ColourListElement element in _backgroundColours)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.colour;
                }
            }

            Debug.LogError("Could not find background colour for environment " + EnvironmentName + "!");
            return Color.magenta;
        }
    }

    public GameObject AmbianceBackground
    {
        get
        {
            foreach (AmbianceBgListElement element in _ambiancePrefabs)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.ambiancePrefab;
                }
            }

            Debug.LogError("Could not find ambiance prefab for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public Color CrevasseColor
    {
        get
        {
            foreach (ColourListElement element in _crevasseColours)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.colour;
                }
            }

            Debug.LogError("Could not find crevasse colour for environment " + EnvironmentName + "!");
            return Color.magenta;
        }
    }

    public Color WallColor
    {
        get
        {
            foreach (ColourListElement element in _wallColours)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.colour;
                }
            }

            Debug.LogError("Could not find wall colour for environment " + EnvironmentName + "!");
            return Color.magenta;
        }
    }

    public Color WallShadowColor
    {
        get
        {
            foreach (ColourListElement element in _wallShadowColours)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.colour;
                }
            }

            Debug.LogError("Could not find wall shadow colour for environment " + EnvironmentName + "!");
            return Color.magenta;
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

    public Sprite PlantSprite
    {
        get
        {
            foreach (SpriteListElement element in _plantSprites)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.sprite;
                }
            }

            Debug.LogError("Could not find plant sprite for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public Sprite UIImage
    {
        get
        {
            foreach (SpriteListElement element in _UIImages)
            {
                if (element.environmentName == EnvironmentName)
                {
                    return element.sprite;
                }
            }

            Debug.LogError("Could not find UI image for environment " + EnvironmentName + "!");
            return null;
        }
    }

    public float ScoreMultiplier
    {
        get
        {
            if(DifficultyLevel >= _scoreMultipliers.Count)
            {
                return 1;
            }

            return _scoreMultipliers[DifficultyLevel];
        }
    }

    public int SessionScore { get; private set; }

    public int BestSessionScore { get; private set; }

    public int TotalScore { get; private set; }

    public int OldTotalScore { get { return TotalScore - SessionScore; } }

    public int MaxLevel { get { return _levelExperience.Count; } }

    //==========================================================================================
    //
    //==========================================================================================

    public void StartGame()
    {
        SessionScore = 0;

        //***************************
        
        SceneManager.LoadSceneAsync("Scenes/TestLevel");
    }

    public void LoadMenu(int levelScore)
    {
        SessionScore = levelScore;

        if (levelScore > BestSessionScore)
        {
            BestSessionScore = levelScore;
        }

        TotalScore += levelScore;

        //***************************
        PlayerPrefs.SetInt("TotalScore", TotalScore);
        PlayerPrefs.SetInt("BestSessionScore", BestSessionScore);

        //***************************
        PlayerCharacterScript player = GameObject.FindObjectOfType<PlayerCharacterScript>();

        var customProperties = new Dictionary<string, object>()
        {
            {"Environment", EnvironmentName },
            {"Stage Score", SessionScore },
            {"Altitude", player.HighestAltitude },
            {"Character", CharacterName },
            {"Last Chunk", player.CurrentChunkName }
        };

        AmplitudeHelper.Instance.LogEvent("Stage End", customProperties);

        //***************************
        SceneManager.LoadSceneAsync("Scenes/Menu");
    }

    public int ComputeLevel(int experience)
    {
        int level = 0;

        for (int i = 0; i < _levelExperience.Count; i++)
        {
            if (experience > _levelExperience[i])
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

    public int GetLevelRequirement(int level)
    {
        if (level > _levelExperience.Count - 1)
        {
            return int.MaxValue;
        }

        return _levelExperience[level];
    }

    //==========================================================================================
    //
    //==========================================================================================

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (this != _instance)
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
    }

    //==========================================================================================
    //
    //==========================================================================================

    void FillTrackingProperties(Dictionary<string, object> properties)
    {
        properties["Total Score"] = TotalScore;
        properties["Best Session Score"] = BestSessionScore;
    }
}

//----------------------------------------------------------------------------------

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

[System.Serializable]
public class SpriteListElement
{
    public string environmentName;
    public Sprite sprite;
}

[System.Serializable]
public class ColourListElement
{
    public string environmentName;
    public Color colour;
}

[System.Serializable]
public class ScoreMultiplierListElement
{
    public int difficulty;
    public float multiplier;
}