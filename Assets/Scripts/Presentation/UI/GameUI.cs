using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameUI : MonoBehaviour, IGameUI
{
    [Inject] private IEventBus _eventBus;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private PanelWin _winPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private TextMeshProUGUI _lifesValueText;
    public void SetLifesValue(int lifes)
    {
        _lifesValueText.text = lifes.ToString();
    }

    public void ShowGameOver()
    {
        HideAll();
        _gameOverPanel.SetActive(true);
    }

    public void ShowWin()
    {
        HideAll();
        _winPanel.Show();
    }

    private void HideAll()
    {
        _gameOverPanel.SetActive(false);
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

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
