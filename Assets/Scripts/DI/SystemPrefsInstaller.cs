using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SystemPrefsInstaller", menuName = "Installers/SystemPrefsInstaller")]
public class SystemPrefsInstaller : ScriptableObjectInstaller<SystemPrefsInstaller>
{
    [SerializeField] private SystemPrefsModel _systemPrefs;
    public override void InstallBindings()
    {
        Container.BindInstance(_systemPrefs);
    }
}