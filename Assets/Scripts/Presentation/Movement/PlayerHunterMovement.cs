using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHunterMovement : MonoBehaviour
{
    private Joystick _joystick;
    private Hunter _hunter;
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _hunter = GetComponent<Hunter>();
        _joystick = FindObjectOfType<Joystick>();
    }

    private void Start()
    {
        
            
    }

    private void Update()
    {
        if (_joystick != null)
        {
            //Debug.Log("Now move on "+_joystick.Direction.ToString());
            _character.Move(_joystick.Direction);
        }
    }
}
