using UnityEngine;
using Zenject;

public class ProjectServicesInstaller : MonoInstaller
{
    [SerializeField] private EffectPlayController _effectPlayPrefub;
    [SerializeField] private VibrationController _vibrationPrefub;
    public override void InstallBindings()
    {
        Container.Bind<IAudioEffect>().To<EffectPlayController>().FromComponentInNewPrefab(_effectPlayPrefub).AsSingle();
        Container.Bind<IVibration>().To<VibrationController>().FromComponentInNewPrefab(_vibrationPrefub).AsSingle();

       /* var gameShop = Container.Resolve<SystemPrefsModel>().ShopType;

        


#if UNITY_EDITOR
        Container.Bind<ICloudGameStats>().To<NoCloudStatImpl>().FromNew().AsSingle().Lazy();
#elif UNITY_WEBGL
        if (gameShop == GameShopType.YANDEX_GAMES)
            Container.Bind<ICloudGameStats>().To<YandexCloudStatsImpl>().FromNew().AsSingle().Lazy();
        else
            Container.Bind<ICloudGameStats>().To<NoCloudStatImpl>().FromNew().AsSingle().Lazy();
#else
Container.Bind<ICloudGameStats>().To<NoCloudStatImpl>().FromNew().AsSingle().Lazy();
#endif

        Container.Bind<StatDataService>().FromNew().AsSingle().Lazy();*/

    }
}