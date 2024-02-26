using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BrakingObjectFactory
{
    private DiContainer _container;
    private BrakingEffectBooster _brakingPrefub;
    public BrakingObjectFactory(DiContainer diContainer, BrakingEffectBooster brakingPrefub)
    {
        _container = diContainer;
        _brakingPrefub = brakingPrefub;
    }

    public BrakingEffectBooster Spawn(GameObject parent)
    {
        var res = _container.InstantiatePrefabForComponent<BrakingEffectBooster>(_brakingPrefub);
        res.SetParentObject(parent);
        return res;
    }
}
