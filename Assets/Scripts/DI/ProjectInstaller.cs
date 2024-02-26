using System.ComponentModel;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameStats>().To<GameStatsLocalImpl>().AsSingle().Lazy();
        Container.Bind<StatDataService>().AsSingle().Lazy();
        Container.Bind<ISettings>().To<SettingsLocalPrefs>().AsSingle().Lazy();
        Container.Bind<IProgressItems>().To<ProgressLocalPrefs>().AsSingle().Lazy();
        Container.Bind<ItemsProgressService>().AsSingle();
        Container.Bind<IData>().To<DataLocalPrefs>().FromNew().AsSingle().NonLazy();
        Container.Bind<GameStatService>().FromNew().AsSingle();
        Container.Bind<ILang>().To<SimpleLocalizationImpl>().FromNew().AsSingle();
        Container.Bind<ISystemInfo>().To<SystemInfoMultiImpl>().FromNew().AsSingle();

       // var platform = Container.Resolve<ISystemInfo>().GetPlatformType();
var gameShop = Container.Resolve<ISystemInfo>().GetSystemPrefs().ShopType;
#if UNITY_EDITOR
Container.Bind<ICloudGameStats>().To<NoCloudStatImpl>().FromNew().AsSingle().Lazy();
#elif UNITY_WEBGL
        switch (gameShop)
        {
            case GameShopType.YANDEX_GAMES:
                Container.Bind<ICloudGameStats>().To<YandexCloudStatsImpl>().FromNew().AsSingle().Lazy();
                break;
            default:
                Container.Bind<ICloudGameStats>().To<NoCloudStatImpl>().FromNew().AsSingle().Lazy();
                break;
        }
#else
    Container.Bind<ICloudGameStats>().To<NoCloudStatImpl>().FromNew().AsSingle().Lazy();
#endif
    }
}