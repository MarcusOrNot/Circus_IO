using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHunterMovement : MonoBehaviour
{
    private Joystick _joystick;

    private void Awake()
    {
        _joystick = FindObjectOfType<Joystick>();
    }

    private void Start()
    {
        
            
    }

    private void Update()
    {
        if (_joystick != null)
        {
            Debug.Log("Now move on "+_joystick.Direction.ToString());
        }
    }
}
