using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

public class SimpleButtonUI : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClick;
    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();

    }
}
