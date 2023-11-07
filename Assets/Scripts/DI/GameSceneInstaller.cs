using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBusController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IControlCharacter>().To<UICharacterController>().FromComponentInHierarchy().AsSingle();
    }
}