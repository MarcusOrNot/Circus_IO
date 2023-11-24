using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SoundsInstaller : MonoInstaller
{
    [SerializeField] private List<SoundEffectModel> _soundEffects;
    public override void InstallBindings()
    {
        Container.Bind<EffectPlayService>().FromNew().AsTransient();
        Container.BindInstance(_soundEffects);
    }
}