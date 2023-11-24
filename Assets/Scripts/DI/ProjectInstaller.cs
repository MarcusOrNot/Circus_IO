using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameStats>().To<GameStatsLocalImpl>().AsSingle().Lazy();
        Container.Bind<ISettings>().To<SettingsLocalPrefs>().AsSingle().Lazy();
    }
}