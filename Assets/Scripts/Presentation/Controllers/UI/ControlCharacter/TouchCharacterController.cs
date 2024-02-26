using UnityEngine;

public class TouchCharacterController : UICharacterController
{
    [SerializeField] private Joystick _joystick;
    public override Vector2 Direction => _joystick.Direction;

    public override void Hide()
    {
        base.Hide();
        _joystick.gameObject.SetActive(false);
    }

    public override void Show() 
    {
        base.Show();
        _joystick.gameObject.SetActive(true);
    }
}
