using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    private Character _character;
    private Hunter _hunter;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _hunter = GetComponent<Hunter>();
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.A)) { _character.Move(Vector2.left); }
        if (Input.GetKey(KeyCode.D)) { _character.Move(Vector2.right); }
        if (Input.GetKey(KeyCode.W)) { _character.Move(Vector2.up); }
        if (Input.GetKey(KeyCode.S)) { _character.Move(Vector2.down); }
        if (Input.GetKey(KeyCode.Space)) { if (_hunter.IsReadyToBoost) _hunter.Boost(); }
        //if (Input.GetKey(KeyCode.A)) { Turn(-_turnSensivity * Time.deltaTime); }
        //if (Input.GetKey(KeyCode.D)) { Turn(_turnSensivity * Time.deltaTime); }
        //if (Input.GetKeyDown(KeyCode.X)) { GetDamage(); }
        //if (Input.GetKeyDown(KeyCode.Space)) { if (_speed == 0) _speed = _model.Speed; else if (_isReadyToBoost) StartCoroutine(Boost()); }
    }
}
