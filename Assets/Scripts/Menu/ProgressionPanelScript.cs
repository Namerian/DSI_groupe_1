using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionPanelScript : MonoBehaviour, IMenuPanel
{
    [SerializeField]
    private float _sliderScoreGainPerSecond = 2000;

    private MenuScript _menu;

    private CanvasGroup _canvasGroup;
    private Text _lastSessionScoreText;
    private Text _bestSessionScoreText;
    private Text _totalScoreText;
    private Text _currentLevelText;
    private Text _nextLevelText;
    private Slider _levelSlider;

    private bool _active = false;
    private bool _updated = false;
    private bool _updating = false;

    private int _currentLevel;
    private float _nextLevelRequiredScore;
    private float _currentTotalScore;
    private float _currentLevelScore;
    private float _targetTotalScore;

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

    void Update()
    {
        if (_updating)
        {
            bool updatingDone = false;
            float scoreGain = _sliderScoreGainPerSecond * Time.deltaTime;

            _currentTotalScore += scoreGain;

            if (_currentTotalScore > _targetTotalScore)
            {
                scoreGain -= (_currentTotalScore - _targetTotalScore);
                _currentTotalScore = _targetTotalScore;
                updatingDone = true;
            }

            _currentLevelScore += scoreGain;

            if (_currentLevelScore >= _nextLevelRequiredScore)
            {
                _currentLevelScore -= _nextLevelRequiredScore;
                _currentLevel++;
                _nextLevelRequiredScore = GameManagerScript.Instance.GetLevelRequirement(_currentLevel);

                _currentLevelText.text = "Lvl " + (_currentLevel + 1);

                if (_currentLevel < GameManagerScript.Instance.MaxLevel)
                {
                    _nextLevelText.text = "Lvl " + (_currentLevel + 2);
                }
                else
                {
                    _nextLevelText.text = "Max Level";

                    updatingDone = true;
                    _currentLevelScore = 0;
                }
            }

            float sliderFill = _currentLevelScore / _nextLevelRequiredScore;
            //Debug.Log("slider update: current level score = " + _currentLevelScore + ";  next level = " + _nextLevelRequiredScore + ";  slider fill = " + sliderFill);
            _levelSlider.value = sliderFill;

            if (updatingDone)
            {
                _updated = true;
                _updating = false;
            }
        }
    }

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

            int currentLevel = GameManagerScript.Instance.ComputeLevel(GameManagerScript.Instance.OldTotalScore);
            _currentLevelText.text = "Lvl " + (currentLevel + 1);

            if (currentLevel < GameManagerScript.Instance.MaxLevel)
            {
                _nextLevelText.text = "Lvl " + (currentLevel + 2);
            }
            else
            {
                _nextLevelText.text = "Max Level";
            }

            if (!_updated && GameManagerScript.Instance.SessionScore > 0 && currentLevel < GameManagerScript.Instance.MaxLevel)
            {
                _updating = true;

                _currentLevel = currentLevel;
                _currentTotalScore = GameManagerScript.Instance.OldTotalScore;
                _currentLevelScore = _currentTotalScore;

                for (int i = 0; i < _currentLevel; i++)
                {
                    _currentLevelScore -= GameManagerScript.Instance.GetLevelRequirement(i);
                }

                _nextLevelRequiredScore = GameManagerScript.Instance.GetLevelRequirement(_currentLevel);
                _targetTotalScore = GameManagerScript.Instance.TotalScore;

                float initialSliderFill = _currentLevelScore / _nextLevelRequiredScore;
                _levelSlider.value = initialSliderFill;

                //Debug.Log("start: total score = " + _currentTotalScore);
                //Debug.Log("start: rest score = " + _currentLevelScore);
                //Debug.Log("start: target score = " + _targetTotalScore);
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

    public void OnButtonBack()
    {
        _menu.SwitchPanel(_menu.LevelSelectionPanel);
    }
}
