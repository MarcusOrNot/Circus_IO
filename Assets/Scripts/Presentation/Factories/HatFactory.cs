using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HatFactory
{
    private DiContainer _container;
    private List<Hat> _hats;
    public HatFactory(DiContainer diContainer, List<Hat> hats) {
        _container = diContainer;
        _hats = hats;
    }
    public Hat Spawn(HatType hatType)
    {
        var prefub = _hats.Find(e => e.HatModel.HatType == hatType);
        if (prefub != null)
        {
            return _container.InstantiatePrefabForComponent<Hat>(prefub);
        }

        return null;
    }
}
