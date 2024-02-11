using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using DG.Tweening;

public class GameUI : MonoBehaviour, IGameUI
{
    [Inject] private IEventBus _eventBus;
    [Inject] private IData _data;
    [SerializeField] private PanelLose _gameOverPanel;
    [SerializeField] private PanelWin _winPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private AdPauseMessage _adPausePanel;
    [SerializeField] private TextMeshProUGUI _lifesValueText;
    [SerializeField] private RateUsUI _rateUsUI;
    [SerializeField] private TextMeshProUGUI _alertMessageText;

    /*private void Awake()
    {
        _adPausePanel.SetOnTimeFinished(ShowAdPause);   
    }*/
    public void SetLifesValue(int lifes)
    {
        _lifesValueText.text = lifes.ToString();
    }

    public void ShowGameOver()
    {
        _adPausePanel.StopMessage();
        HideAll();
        _gameOverPanel.ShowPanel();
    }

    public void ShowWin()
    {
        _adPausePanel.StopMessage();
        HideAll();
        _winPanel.Show();
        _rateUsUI.ShowByCondition1();
    }

    private void HideAll()
    {
        _gameOverPanel.HidePanel();
        _winPanel.Hide();
        _pausePanel.SetActive(false);
        CloseAlertMessage();
    }

    public void RestartLevel()
    {
        Level.Instance.RestartLevel();
    }

    public void Continue()
    {
        HideAll();
        _eventBus.NotifyObservers(GameEventType.GAME_CONTINUE);
    }

    public void PauseGame()
    {
        ShowPause();
        _eventBus.NotifyObservers(GameEventType.GAME_PAUSED);
    }

    public void ShowPause()
    {
        HideAll();
        _pausePanel.SetActive(true);
    }

    public void ShowAdPause()
    {
        HideAll();
        _eventBus.NotifyObservers(GameEventType.GAME_AD_PAUSED);
    }

    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        Utils.OpenScene(SceneType.MAIN_MENU);
    }

    public void ShowAlertMessage(string alertText)
    {
        _alertMessageText.text = alertText;
        _alertMessageText.gameObject.SetActive(true);
    }

    public void CloseAlertMessage()
    {
        _alertMessageText.gameObject.SetActive(false);
    }

    public void ShowCounterStart(int delaySeconds)
    {
        HideAll();
        ShowAlertMessage(delaySeconds.ToString());
        DOTween.To((x) => _alertMessageText.text = (Mathf.FloorToInt(x+1)).ToString(), delaySeconds, 0, delaySeconds).SetUpdate(true).OnComplete(() =>
        {
            Continue();
            //Debug.Log("Ready");
        }).PlayForward();
    }
}
