using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public float bestScore, totalScore, scoreToAdd;
    public Text currentLevelText, nextLevelText;
    public Slider progressionJauge;
    public float[] levelsXP;
    public int currentLevel;
    [SerializeField]
    private GameObject activePanel, progressPanel;

	// Use this for initialization
	void Start ()
    {
        //StartCoroutine(JaugeFill());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void GameResults()
    {
        activePanel.SetActive(false);
        progressPanel.SetActive(true);
        activePanel = progressPanel;
    }

    public void ChangePanel(GameObject panel)
    {
        activePanel.SetActive(false);
        panel.SetActive(true);
        activePanel = panel;
    }

    
    IEnumerator JaugeFill() //Ne fonctionne pas très bien
    {
        float addToJauge = 0;
        float addToJaugeEachFrame = scoreToAdd / 100;

        progressionJauge.maxValue = levelsXP[currentLevel];
        
        while(addToJauge <= scoreToAdd)
        {
            progressionJauge.value += addToJaugeEachFrame;
            addToJauge += addToJaugeEachFrame;
            Debug.Log(addToJauge);

            if(progressionJauge.value >= progressionJauge.maxValue)
            {
                currentLevel += 1;
                currentLevelText.text = "Level " + currentLevel;
                nextLevelText.text = "Level " + (currentLevel + 1);
                progressionJauge.maxValue = levelsXP[currentLevel];
                progressionJauge.value = 0;
            }
            yield return null;
        }
    }

    public void LoadLevel(int difficulty)
    {
        GameManagerScript.Instance.DifficultyLevel = difficulty;
        GameManagerScript.Instance.StartGame();
    }

    public void ChangeCharacter(string name)
    {
        GameManagerScript.Instance.CharacterName = name;
    }
}
