using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionPanelScript : MonoBehaviour, IMenuPanel
{
    private MenuScript _menu;

    private CanvasGroup _canvasGroup;
    private Text _lastSessionScoreText;
    private Text _bestSessionScoreText;
    private Text _totalScoreText;
    private Text _currentLevelText;
    private Text _nextLevelText;
    private Slider _levelSlider;

    private bool _active;

    void Awake()
    {
        _menu = this.transform.parent.GetComponent<MenuScript>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _lastSessionScoreText = this.transform.Find("LevelPanel/LastSessionScoreText").GetComponent<Text>();
        _bestSessionScoreText = this.transform.Find("LevelPanel/BestSessionScoreText").GetComponent<Text>();
        _totalScoreText = this.transform.Find("LevelPanel/TotalScoreText").GetComponent<Text>();
        _currentLevelText = this.transform.Find("LevelPanel/CurrentLevelText").GetComponent<Text>();
        _nextLevelText = this.transform.Find("LevelPanel/NextLevelText").GetComponent<Text>();
        _levelSlider = this.transform.Find("LevelPanel/Slider").GetComponent<Slider>();

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

            _lastSessionScoreText.text = "Last Session Score: " + GameManagerScript.Instance.SessionScore;
            _bestSessionScoreText.text = "Best Session Score: " + GameManagerScript.Instance.BestSessionScore;
            _totalScoreText.text = "Total Score: " + GameManagerScript.Instance.TotalScore;
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

    public void OnButtonBack()
    {
        _menu.SwitchPanel(_menu.LevelSelectionPanel);
    }
}
