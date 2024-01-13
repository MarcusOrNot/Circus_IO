using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameStats>().To<GameStatsLocalImpl>().AsSingle().Lazy();
        Container.Bind<ISettings>().To<SettingsLocalPrefs>().AsSingle().Lazy();
        Container.Bind<IProgressItems>().To<ProgressLocalPrefs>().AsSingle().Lazy();
        Container.Bind<ItemsProgressService>().AsSingle();
        Container.Bind<IData>().To<DataLocalPrefs>().FromNew().AsSingle().NonLazy();
        Container.Bind<GameStatService>().FromNew().AsSingle();
    }
}