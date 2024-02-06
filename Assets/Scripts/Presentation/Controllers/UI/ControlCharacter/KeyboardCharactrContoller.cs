using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCharactrContoller : UICharacterController
{
    private Vector2 _currentVector = Vector2.zero;
    [SerializeField] private KeyboardInput _keyInput;
    public override Vector2 Direction
    {
        get
        {
            return _currentVector;
        }
    }

    private void Update()
    {
        Vector2 newVector = _keyInput.Direction;
        /*if (Input.GetKey(KeyCode.W))
            newVector += Vector2.up;
        if (Input.GetKey(KeyCode.A))
            newVector += Vector2.left;
        if (Input.GetKey(KeyCode.D))
           newVector += Vector2.right;
        if (Input.GetKey(KeyCode.S))
            newVector += Vector2.down;*/

        if (_currentVector==Vector2.zero || newVector==Vector2.zero)
        {
            _currentVector = newVector;
        }
        else
        _currentVector = Vector2.Lerp(_currentVector, newVector, 4*Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
            _actionButton.Click();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _debafButton.Click();
    }
}
