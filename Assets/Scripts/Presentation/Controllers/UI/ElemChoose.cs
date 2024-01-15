using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Image))]
public class ElemChoose : MonoBehaviour, IPointerClickHandler
{
    private Action _onClick;
    private bool _isChosen = true;
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public bool Chosen
    {
        set
        {
            _isChosen = value;
            _image.color = _isChosen ? Color.white : Color.black; 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke();
    }

    public void SetOnClick(Action onClick)
    {
        _onClick = onClick;
    }
}
