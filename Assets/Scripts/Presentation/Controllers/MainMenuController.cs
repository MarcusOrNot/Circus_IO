using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Inject] private IGameStats _gameStats;
    [SerializeField] private InputField _nameField;
    [SerializeField] private StartGameUI _startGameUI;
    private void Start()
    {
        
        _nameField.text = _gameStats.PlayerName;
    }
    public void StartGame()
    {
        _gameStats.PlayerName = _nameField.text;
        _startGameUI.StartRoyalGame();
        //SceneManager.LoadScene("GameScene");
        //StartCoroutine(ConnectorCoro(5));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    /*private void OnDestroy()
    {
        StopAllCoroutines();
    }*/
}
