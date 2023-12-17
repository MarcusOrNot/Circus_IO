using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "DataInstaller", menuName = "Installers/DataInstaller")]
public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
{
    [SerializeField] private List<HatItemModel> _hatItems;
    public override void InstallBindings()
    {
        Container.BindInstance(_hatItems);
    }
}