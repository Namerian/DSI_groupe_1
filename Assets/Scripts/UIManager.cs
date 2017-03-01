using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour {

    public Text scoreText, comboText, addScoreText, altitudeText;
    public float score; 
    public int combo;
    public Color[] comboColors;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        score += combo;
        UpdateScoreText();
	}

    public void UpdateScoreText()
    {
        scoreText.text = "" + score;
    }

    public void UpdateCombo(int comb)
    {
        if (comb > combo) comboText.transform.DOShakePosition(0.5f, comb*2, 10);
        combo = comb;
        comboText.text = "x" + combo;
        comboText.fontSize = (60 + (8 * combo));
        comboText.color = comboColors[combo];
    }

    public void UpdateAltitude(float altitude)
    {
        altitudeText.text = "" + Mathf.Round(altitude) + "m";
    }

    public void AddScore(int scoreToAdd)
    {
        StopCoroutine(AddScoreCoroutine(scoreToAdd));
        StartCoroutine(AddScoreCoroutine(scoreToAdd));
    }

    IEnumerator AddScoreCoroutine(int scoreToAdd)
    {
        addScoreText.text = "+" + scoreToAdd;
        addScoreText.transform.DOScaleY(1f, 0.1f);
        yield return null;
        addScoreText.transform.DOScaleY(0.5f, 0.5f);
        score += scoreToAdd;
        yield return new WaitForSeconds(1);
        addScoreText.transform.DOScaleY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }
}
