using UnityEngine;
using Zenject;

public class AdsInstaller : MonoInstaller
{
    //[SerializeField] private AppodealAds _appodealPrefub;
    public override void InstallBindings()
    {
        #if UNITY_ANDROID || UNITY_EDITOR
            //Container.Bind<IAds>().To<AppodealAds>().FromComponentOn(_appodealPrefub.gameObject).AsSingle().NonLazy();
            Container.Bind<IAds>().To<AppodealAds>().FromNew().AsSingle();
        #elif UNITY_WEBGL
            Container.Bind<IAds>().To<YandexAds>().FromNew().AsSingle();
        #endif
        //Container.Bind<IData>().To<DataLocalPrefs>().FromNew().AsSingle().NonLazy();
        Container.Bind<AdService>().FromNew().AsSingle();
    }
}