using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShopHatController : MonoBehaviour
{
    //[SerializeField] private GameObject _menuHunters;
    [Inject] private ItemsProgressService _progress;
    [Inject] private HatShopFactory _hatsFactory;
    [SerializeField] private Transform _hatsPlace;
    private List<HatShopItem> _hats = new List<HatShopItem>();
    private int _selectedHunter = 0;
    private int _currentHat = 0;
    private void Start()
    {
        var hats = _hatsFactory.SpawnItems();
        foreach (var hat in hats)
        {
            _hats.Add(hat);
            hat.transform.parent = _hatsPlace;
            hat.SetOnClicked(() =>
            {
                foreach (var item in _hats)
                {
                    if (item != hat)
                        item.Selected = false;
                }
            });
        }
    }
}
