using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHunterMovement : MonoBehaviour
{
    [Inject] private IControlCharacter _controller;
    private Hunter _hunter;

    private void Awake()
    {
        _hunter = GetComponent<Hunter>();
        if (_controller!=null)
        {
            _controller.SetOnActionClicked(() =>
            {
                _hunter.Boost();
            });
        }
    }

    private void Start()
    {
        
            
    }

    private void Update()
    {
        if (_controller != null)
        {
            //Debug.Log("Now move on "+_joystick.Direction.ToString());
            _hunter.Move(_controller.Direction);
        }
        else
        {
            Debug.Log("Requires Control Character!!!");
        }
    }
}
