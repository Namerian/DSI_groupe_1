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
    private Outline _char1Outline;
    private Outline _char2Outline;
    private Outline _char3Outline;
    private CanvasGroup _loadingPanelCanvasGroup;

    private bool _active;

    void Awake()
    {
        _menu = this.transform.parent.GetComponent<MenuScript>();

        _canvasGroup = GetComponent<CanvasGroup>();
        _stage2Button = this.transform.Find("PanelNiveaux/ButtonLevel2").GetComponent<Button>();
        _stage3Button = this.transform.Find("PanelNiveaux/ButtonLevel3").GetComponent<Button>();
        _char2Button = this.transform.Find("PanelPersos/ButtonChat").GetComponent<Button>();
        _char3Button = this.transform.Find("PanelPersos/ButtonPerso3").GetComponent<Button>();
        _char1Outline = this.transform.Find("PanelPersos/ButtonPoulpe").GetComponent<Outline>();
        _char2Outline = this.transform.Find("PanelPersos/ButtonChat").GetComponent<Outline>();
        _char3Outline = this.transform.Find("PanelPersos/ButtonPerso3").GetComponent<Outline>();
        _loadingPanelCanvasGroup = this.transform.Find("PanelLoading").GetComponent<CanvasGroup>();

        _stage2Button.interactable = false;
        _stage3Button.interactable = false;
        _char2Button.interactable = false;
        _char3Button.interactable = false;
        _char1Outline.enabled = false;
        _char2Outline.enabled = false;
        _char3Outline.enabled = false;

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

            OnCharacterButton(0);
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
        _loadingPanelCanvasGroup.alpha = 1;

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

    public void OnCharacterButton(int id)
    {
        switch (id)
        {
            case 0:
                GameManagerScript.Instance.CharacterName = "Poulpe";
                _char1Outline.enabled = true;
                _char2Outline.enabled = false;
                _char3Outline.enabled = false;
                break;
            case 1:
                GameManagerScript.Instance.CharacterName = "Chat";
                _char1Outline.enabled = false;
                _char2Outline.enabled = true;
                _char3Outline.enabled = false;
                break;
        }
    }
}
