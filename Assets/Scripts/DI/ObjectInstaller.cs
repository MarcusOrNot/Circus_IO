using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectInstaller : MonoInstaller
{
    [SerializeField] private List<Entity> _entities;
    [SerializeField] private List<Booster> _boosters;
    [SerializeField] private List<Hat> _hats;
    [SerializeField] private List<Hunter> _hunters;
    [SerializeField] private HatShopItem _hatShopPrefab;
    [SerializeField] private BrakingEffectBooster _brakingPrefub;
    public override void InstallBindings()
    {
        Container.Bind<EntityFactory>().FromNew().AsSingle();
        Container.BindInstance(_entities);

        Container.Bind<BoosterFactory>().FromNew().AsSingle();
        Container.BindInstance(_boosters);

        Container.Bind<HatFactory>().FromNew().AsSingle();
        Container.BindInstance(_hats);

        Container.Bind<HunterFactory>().FromNew().AsSingle();
        Container.BindInstance(_hunters);

        Container.Bind<HatShopFactory>().FromNew().AsSingle();
        Container.BindInstance(_hatShopPrefab);

        Container.Bind<BrakingObjectFactory>().FromNew().AsSingle();
        Container.BindInstance(_brakingPrefub);
    }
}