using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHunter : MonoBehaviour, IPlayer
{
    [Inject] private IControlCharacter _controller;
    [Inject] private IGameUI _gameUI;
    private Hunter _hunter;

    public Vector3 GetPosition() => transform.position;

    private void Awake()
    {
        _hunter = GetComponent<Hunter>();
        if (_controller != null)
        {
            _controller.SetOnActionClicked(() =>
            {
                _hunter.Boost();
            });
            _hunter.SetOnHealthChanged((lifes) =>
            {
                _gameUI.SetLifesValue(lifes);
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