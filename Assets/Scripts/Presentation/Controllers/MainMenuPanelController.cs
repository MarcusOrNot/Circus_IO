using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Asteroids;

public class MainMenuPanelController : MonoBehaviour
{
    [Inject] private IGameStats _gameStats;
    [SerializeField] private InputField _nameField;
    private void Start()
    {
        _nameField.text = _gameStats.PlayerName;
    }
    public void SaveName()
    {
        _gameStats.PlayerName = _nameField.text;
    }
}
