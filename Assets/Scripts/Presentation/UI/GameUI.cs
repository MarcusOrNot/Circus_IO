using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameUI : MonoBehaviour, IGameUI
{
    [Inject] private IEventBus _eventBus;
    [SerializeField] private PanelLose _gameOverPanel;
    [SerializeField] private PanelWin _winPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private AdPauseMessage _adPausePanel;
    [SerializeField] private TextMeshProUGUI _lifesValueText;

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
    }

    private void HideAll()
    {
        _gameOverPanel.HidePanel();
        _winPanel.Hide();
        _pausePanel.SetActive(false);
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
        _eventBus.NotifyObservers(GameEventType.GAME_AD_PAUSED);
    }

    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        Utils.OpenScene(SceneType.MAIN_MENU);
    }
}
