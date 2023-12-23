using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    //[SerializeField] private EffectPlayController _effectComponent;
    [SerializeField] private MusicPlayController _musicComponent;
    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBusController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IControlCharacter>().To<UICharacterController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IGameUI>().To<GameUI>().FromComponentInHierarchy().AsSingle();
        //Container.Bind<IAudioEffect>().To<EffectPlayController>().FromInstance(_effectComponent).AsSingle();
        Container.Bind<IMusicPlayer>().To<MusicPlayController>().FromInstance(_musicComponent).AsSingle();
        //Container.Bind<IAudioEffect>().To<EffectPlayController>().FromComponentInHierarchy().AsSingle();
        //Container.Bind<IMusicPlayer>().To<MusicPlayController>().FromComponentInHierarchy().AsSingle();
    }
}