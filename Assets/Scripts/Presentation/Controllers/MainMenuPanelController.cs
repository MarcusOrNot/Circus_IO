using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Asteroids;

public class MainMenuPanelController : MonoBehaviour
{
    [Inject] private ISettings _settings;
    [SerializeField] private BubbleForm _hatObject;
    [SerializeField] private InputField _nameField;
    private void Start()
    {
        _nameField.text = _settings.PlayerName;
    }
    public void SaveName()
    {
        _settings.PlayerName = _nameField.text;
    }
    private void OnEnable()
    {
        _hatObject.SetHat(_settings.ChosenHat);
    }
}
