using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HatShopFactory
{
    private DiContainer _container;
    private List<HatItemModel> _hatItemModels;
    private HatShopItem _hatItemPrefab;
    public HatShopFactory(DiContainer diContainer, List<HatItemModel> hatModels, HatShopItem hatItemPrefab)
    {
        _container = diContainer;
        _hatItemModels = hatModels;
        _hatItemPrefab = hatItemPrefab;
    }
    public List<HatShopItem> SpawnItems()
    {
        List<HatShopItem> res = new List<HatShopItem>();
        foreach (var item in _hatItemModels)
        {
            var spawned = _container.InstantiatePrefabForComponent<HatShopItem>(_hatItemPrefab);
            spawned.SetModel(item);
            res.Add(spawned);
        }
        return res;
    }
}
