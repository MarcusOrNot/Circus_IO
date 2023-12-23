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
    }
}