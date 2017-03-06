using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionPanelScript : MonoBehaviour, IMenuPanel
{
    private MenuScript _menu;

    private CanvasGroup _canvasGroup;
    private Button _stage2Button;
    private Button _stage3Button;
    private Button _char2Button;
    private Button _char3Button;

    private bool _active;

    void Awake()
    {
        _menu = this.transform.parent.GetComponent<MenuScript>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _stage2Button = this.transform.Find("PanelNiveaux/ButtonLevel2").GetComponent<Button>();
        _stage3Button = this.transform.Find("PanelNiveaux/ButtonLevel3").GetComponent<Button>();
        _char2Button = this.transform.Find("PanelPersos/ButtonChat").GetComponent<Button>();
        _char3Button = this.transform.Find("PanelPersos/ButtonPerso3").GetComponent<Button>();

        _stage2Button.interactable = false;
        _stage3Button.interactable = false;
        _char2Button.interactable = false;
        _char3Button.interactable = false;

        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    /*void Update()
    {

    }*/

    public void OnEnter()
    {
        if (!_active)
        {
            _active = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            int level = GameManagerScript.Instance.ComputeLevel(GameManagerScript.Instance.TotalScore);

            if (level >= 4)
            {
                _stage2Button.interactable = true;
            }

            if (level >= 9)
            {
                _char2Button.interactable = true;
            }

            if (level >= 14)
            {
                _stage3Button.interactable = true;
            }
        }
    }

    public void OnExit()
    {
        if (_active)
        {
            _active = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnBackButton()
    {
        _menu.SwitchPanel(_menu.TitlePanel);
    }

    public void OnProgressButton()
    {
        _menu.SwitchPanel(_menu.ProgressionPanel);
    }

    public void OnLevelButton(int difficulty)
    {
        GameManagerScript.Instance.DifficultyLevel = difficulty;

        _canvasGroup.interactable = false;

        switch (difficulty)
        {
            case 0:
                GameManagerScript.Instance.EnvironmentName = "Ocean";
                break;
            case 1:
                GameManagerScript.Instance.EnvironmentName = "Jungle";
                break;
            case 2:
                GameManagerScript.Instance.EnvironmentName = "Mountain";
                break;
        }

        GameManagerScript.Instance.StartGame();
    }

    public void OnCharacterButton(string name)
    {
        GameManagerScript.Instance.CharacterName = name;
    }
}
