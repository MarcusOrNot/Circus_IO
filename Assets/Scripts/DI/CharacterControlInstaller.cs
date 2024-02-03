using UnityEngine;
using Zenject;

public class CharacterControlInstaller : MonoInstaller
{
    [SerializeField] private TouchCharacterController _touchController;
    public override void InstallBindings()
    {
        var control = Container.Resolve<ISystemInfo>().GetControlType();
        Debug.Log("Control type is "+control.ToString());
        Container.Bind<IControlCharacter>().To<TouchCharacterController>().FromComponentInNewPrefab(_touchController).AsSingle();
    }
}