using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using Zenject;

public class SimpleButtonUI : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IAudioEffect _effect;
    [SerializeField] private bool _clickSound = true;
    public UnityEvent OnClick;
    //private TextMeshProUGUI _text;
    private void Awake()
    {
        //_text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
        if (_clickSound == true)
            _effect.PlayEffect(SoundEffectType.MENU_CLICK);
    }
}
