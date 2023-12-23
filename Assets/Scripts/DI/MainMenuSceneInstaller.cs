using UnityEngine;
using Zenject;

public class MainMenuSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<IAudioEffect>().To<EffectPlayController>().FromComponentInHierarchy().AsSingle();
    }
}