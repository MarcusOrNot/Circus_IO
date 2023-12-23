using UnityEngine;
using Zenject;

public class AdsInstaller : MonoInstaller
{
    [SerializeField] private AppodealAds _appodealPrefub;
    public override void InstallBindings()
    {
        Container.Bind<IAds>().To<AppodealAds>().FromComponentOn(_appodealPrefub.gameObject).AsSingle().NonLazy();
        //Container.Bind<IData>().To<DataLocalPrefs>().FromNew().AsSingle().NonLazy();
        Container.Bind<AdService>().FromNew().AsSingle();
    }
}