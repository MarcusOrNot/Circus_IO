using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayersInfoInstaller : MonoInstaller
{
    [SerializeField] private List<PlayerInfoModel> _playersInfo;
    public override void InstallBindings()
    {
        Container.Bind<PlayersInfoService>().FromNew().AsSingle().Lazy();
        Container.BindInstances(_playersInfo);
    }
}