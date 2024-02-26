using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private ButtonKey _rightKey;
    [SerializeField] private ButtonKey _leftKey;
    [SerializeField] private ButtonKey _upKey;
    [SerializeField] private ButtonKey _downKey;
    private Vector2 _currentDirection = Vector2.zero;
    private void Start()
    {
        _rightKey.SetOnKeyPressed(() => { _currentDirection += Vector2.right; });
        _leftKey.SetOnKeyPressed(() => { _currentDirection += Vector2.left; });
        _upKey.SetOnKeyPressed(() => { _currentDirection += Vector2.up; });
        _downKey.SetOnKeyPressed(() => { _currentDirection += Vector2.down; });
    }

    private void Update()
    {
        _currentDirection = Vector2.zero;
    }

    public Vector2 Direction=> _currentDirection;

}
