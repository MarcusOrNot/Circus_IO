using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

public class SoundSwitcher : MonoBehaviour, IPointerClickHandler
{
    [Inject] private ISettings _settings;
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;
    public void OnPointerClick(PointerEventData eventData)
    {

        State = !State;
        InvokeState();
    }

    private void Start()
    {
        RefreshVisual();
    }

    private bool State
    {
        get => _settings.SoundOn;
        set
        {
            _settings.SoundOn = value;
            RefreshVisual();
        }
    }

    private void RefreshVisual()
    {
        _soundImage.sprite = State ? _soundOnSprite : _soundOffSprite;
    }

    private void InvokeState()
    {
        _settings.NotifyObservers(SettingType.SOUND);
    }
}
