using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HatShopItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _hatImage;
    [SerializeField] private Image _selectionRect;
    [SerializeField] private GameObject _priceBack;
    [SerializeField] private TextMeshProUGUI _priceValueText;
    private HatItemModel _model;
    private bool _isSelected = false;
    private bool _priceless = false;
    private Action _onSelect = null;

    public void SetModel(HatItemModel model)
    {
        _model = model;
        _hatImage.sprite = _model.HatSprite;
        _priceValueText.text = _model.ItemBlock.Value.ToString();
    }

    public HatItemModel Model => _model;

    public bool Selected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            _selectionRect.gameObject.SetActive(value);
        }
    }

    public bool Priceless
    {
        get => _priceless;
        set
        {
            _priceless = value;
            _priceBack.gameObject.SetActive(!value);
        }
    }

    public void SetOnClicked(Action onSelect)
    {
        _onSelect = onSelect;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Selected) return;
        Selected = true; //!Selected;
        _onSelect?.Invoke();
    }
}
