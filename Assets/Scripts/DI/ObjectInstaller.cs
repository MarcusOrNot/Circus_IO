using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectInstaller : MonoInstaller
{
    [SerializeField] private List<Entity> _entities;
    public override void InstallBindings()
    {
        Container.Bind<EntityFactory>().FromNew().AsSingle();
        Container.BindInstance(_entities);
    }
}