using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HunterFactory
{
    private DiContainer _container;
    private List<Hunter> _hunters;
    public HunterFactory(DiContainer diContainer, List<Hunter> hunters)
    {
        _container = diContainer;
        _hunters = hunters;
    }

    public Hunter Spawn(HunterType hunterType)
    {
        var prefub = _hunters.Find(e => e.Model.HunterType == hunterType);
        if (prefub != null)
        {
            return _container.InstantiatePrefabForComponent<Hunter>(prefub);
        }

        return null;
    }
}
