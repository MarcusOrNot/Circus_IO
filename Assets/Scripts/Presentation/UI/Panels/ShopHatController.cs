using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.Events;
using Zenject;

public class ShopHatController : MonoBehaviour
{
    //[SerializeField] private GameObject _menuHunters;
    [Inject] private IGameStats _stats;
    [Inject] private ISettings _settings;
    [Inject] private ItemsProgressService _progress;
    [Inject] private HatShopFactory _hatsFactory;
    [Inject] private IAudioEffect _effect;
    [Inject] private IVibration _vibro;
    [SerializeField] private Transform _hatsPlace;
    [SerializeField] private BubbleForm _hatObject;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonPutOn;
    [SerializeField] private TextMeshProUGUI _priceText;
    [Inject] private IAudioEffect _audioEffect;
    private List<HatShopItem> _hats = new List<HatShopItem>();
    //private int _selectedHunter = 0;
    private HatShopItem _currentHat = null;
    //private UnityEvent _onBuy;
    private void Start()
    {
        HideActionButtons();
        var hats = _hatsFactory.SpawnItems();
        foreach (var hat in hats)
        {
            bool isPriceless = hat.Model.ItemBlock.Value == 0 || _progress.IsHatOpened(hat.Model.Hat);
            hat.Priceless = isPriceless;
                
            _hats.Add(hat);
            hat.transform.SetParent(_hatsPlace);
            hat.SetOnClicked(() =>
            {
                _vibro.Play();
                _currentHat = hat;
                foreach (var item in _hats)
                {
                    if (item != hat)
                        item.Selected = false;
                }
                _hatObject.SetHat(hat.Model.Hat);
                SetBuyOrFree(!isPriceless);
                if (!isPriceless)
                {
                    _priceText.text = hat.Model.ItemBlock.Value.ToString();
                }
                if (hat.Selected && _settings.ChosenHat == hat.Model.Hat)
                    HideActionButtons();
            });
        }
    }
    private void SetBuyOrFree(bool isPrice)
    {
        _buttonBuy.gameObject.SetActive(isPrice);
        _buttonPutOn.gameObject.SetActive(!isPrice);
    }
    public void HideActionButtons()
    {
        _buttonBuy.gameObject.SetActive(false);
        _buttonPutOn.gameObject.SetActive(false);
    }

    public void BuyItem()
    {
        if (_currentHat == null) return;
        if (_stats.GetStat(GameStatsType.COINS)>=_currentHat.Model.ItemBlock.Value)
        {
            _stats.SetGameStat(GameStatsType.COINS, _stats.GetStat(GameStatsType.COINS)- _currentHat.Model.ItemBlock.Value);
            _progress.OpenHat(_currentHat.Model.Hat);
            _currentHat.Priceless = true;
            PutOn();
            _audioEffect.PlayEffect(SoundEffectType.PURCHASE_HAT);
            _vibro.PlayMillis(300);
        }
        else
        {
            _audioEffect.PlayEffect(SoundEffectType.CLICK_NEGATIVE);
            _vibro.Play();
        }
    }
    public void PutOn()
    {
        if (_currentHat == null) return;
        HideActionButtons();
        _settings.ChosenHat = _currentHat.Model.Hat;
    }

    private void OnEnable()
    {
        HideActionButtons();
        foreach (var hat in _hats)
            hat.Selected = false;
    }
}
