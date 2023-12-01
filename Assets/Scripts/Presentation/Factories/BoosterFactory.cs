using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoosterFactory
{
    private DiContainer _container;
    private List<Booster> _boosters;
    public BoosterFactory(DiContainer diContainer, List<Booster> boosters)
    {
        _container = diContainer;
        _boosters = boosters;
    }

    public Booster Spawn(BoosterType boosterType)
    {
        //GameObjectCreationParameters paramsNow = new GameObjectCreationParameters();
        //paramsNow.
        var prefub = _boosters.Find(e => e.GetBoosterType() == boosterType);
        if (prefub != null)
        {
            //return _container.InstantiatePrefabForComponent<Booster>(prefub);
            var newPrefub = _container.InstantiatePrefab(prefub);
            return newPrefub.GetComponent<Booster>();
        }

        return null;
    }
}
