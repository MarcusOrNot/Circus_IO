using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController3D : MonoBehaviour
{
    [Inject] private IGameStats _gameStats;
    [SerializeField] private StartGameUI _startGameUI;
    [SerializeField] private TextMeshProUGUI _coinsCountValue;
    private void Start()
    {
        _coinsCountValue.text = _gameStats.GetStat(GameStatsType.COINS).ToString();
    }
    /*public void StartGame()
    {
        _gameStats.PlayerName = _nameField.text;
        _startGameUI.StartRoyalGame();
        //SceneManager.LoadScene("GameScene");
        //StartCoroutine(ConnectorCoro(5));
    }*/
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ShowNextHat()
    {

    }
}
