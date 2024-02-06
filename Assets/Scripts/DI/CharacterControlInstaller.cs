using UnityEngine;
using Zenject;

public class CharacterControlInstaller : MonoInstaller
{
    [SerializeField] private TouchCharacterController _touchController;
    [SerializeField] private KeyboardCharactrContoller _keyboardCharactrContoller;
    public override void InstallBindings()
    {
        var control = Container.Resolve<ISystemInfo>().GetControlType();
        switch (control)
        {
            case ControlType.TOUCH_SCREEN:
                Container.Bind<IControlCharacter>().To<TouchCharacterController>().FromComponentInNewPrefab(_touchController).AsSingle();
                break;
            case ControlType.KEYBOARD:
                Container.Bind<IControlCharacter>().To<KeyboardCharactrContoller>().FromComponentInNewPrefab(_keyboardCharactrContoller).AsSingle();
                break;
            case ControlType.KEYBOARD_MOUSE:
                Container.Bind<IControlCharacter>().To<KeyboardCharactrContoller>().FromComponentInNewPrefab(_keyboardCharactrContoller).AsSingle();
                break;
        }
    }
}