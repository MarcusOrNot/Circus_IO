using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShopHatController : MonoBehaviour
{
    //[SerializeField] private GameObject _menuHunters;
    [Inject] private ISettings _settings;
    [Inject] private ItemsProgressService _progress;
    [Inject] private HatShopFactory _hatsFactory;
    [SerializeField] private Transform _hatsPlace;
    [SerializeField] private BubbleForm _hatObject;
    private List<HatShopItem> _hats = new List<HatShopItem>();
    private int _selectedHunter = 0;
    private int _currentHat = 0;
    private void Start()
    {
        var hats = _hatsFactory.SpawnItems();
        foreach (var hat in hats)
        {
            _hats.Add(hat);
            hat.transform.SetParent(_hatsPlace);
            hat.SetOnClicked(() =>
            {
                foreach (var item in _hats)
                {
                    if (item != hat)
                        item.Selected = false;
                }
                _settings.ChosenHat = hat.Model.Hat;
                _hatObject.SetHat(hat.Model.Hat);
            });
        }
    }
}
