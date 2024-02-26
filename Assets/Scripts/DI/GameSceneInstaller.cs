using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    //[SerializeField] private EffectPlayController _effectComponent;
    [SerializeField] private MusicPlayController _musicComponent;
    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBusController>().FromComponentInHierarchy().AsSingle();
        //Container.Bind<IControlCharacter>().To<UICharacterController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IGameUI>().To<GameUI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IMobSpawner>().To<MobsSpawner>().FromComponentInHierarchy().AsSingle();
        //Container.Bind<IAudioEffect>().To<EffectPlayController>().FromInstance(_effectComponent).AsSingle();
        Container.Bind<IMusicPlayer>().To<MusicPlayController>().FromInstance(_musicComponent).AsSingle();
        //Container.Bind<IAudioEffect>().To<EffectPlayController>().FromComponentInHierarchy().AsSingle();
        //Container.Bind<IMusicPlayer>().To<MusicPlayController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelStatService>().FromNew().AsSingle();
        Container.Bind<IZoneController>().To<DamageZoneConroller>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ILevelInfo>().To<Level>().FromComponentInHierarchy().AsSingle();
    }

    private void OnDestroy()
    {
        Container.Resolve<LevelStatService>().UnregisterService();
    }
}