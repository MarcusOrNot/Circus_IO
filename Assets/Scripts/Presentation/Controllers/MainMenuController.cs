using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Inject] private IGameStats _gameStats;
    [SerializeField] private InputField _nameField;
    private void Start()
    {
        
        _nameField.text = _gameStats.PlayerName;
    }
    public void StartGame()
    {
        _gameStats.PlayerName = _nameField.text;
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
