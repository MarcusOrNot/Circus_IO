using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HatShopItem : MonoBehaviour
{
    [SerializeField] private Image _hatImage;
    [SerializeField] private TextMeshProUGUI _priceValueText;
    private HatItemModel _model;

    public void SetModel(HatItemModel model)
    {
        _model = model;
        _hatImage.sprite = _model.HatSprite;
        _priceValueText.text = _model.price.ToString();
    }
}
