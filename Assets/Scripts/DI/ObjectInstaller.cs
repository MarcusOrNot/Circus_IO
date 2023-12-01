using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectInstaller : MonoInstaller
{
    [SerializeField] private List<Entity> _entities;
    [SerializeField] private List<Booster> _boosters;
    public override void InstallBindings()
    {
        Container.Bind<EntityFactory>().FromNew().AsSingle();
        Container.BindInstance(_entities);

        Container.Bind<BoosterFactory>().FromNew().AsSingle();
        Container.BindInstance(_boosters);
    }
}