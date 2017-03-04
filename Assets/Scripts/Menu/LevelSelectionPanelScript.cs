using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionPanelScript : MonoBehaviour, IMenuPanel
{
    private MenuScript _menu;

    private CanvasGroup _canvasGroup;

    private bool _active;

    void Awake()
    {
        _menu = this.transform.parent.GetComponent<MenuScript>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnter()
    {
        if (!_active)
        {
            _active = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
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
