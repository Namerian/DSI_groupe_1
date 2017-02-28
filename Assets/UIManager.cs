using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //Ce code sert à rien pour l'instant, on pourra le virer si on veut faire d'une autre manière


    public Text scoreText;
    public float score;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        score += 1;
        UpdateScoreText();
	}

    public void UpdateScoreText()
    {
        scoreText.text = "" + score;
    }
}
