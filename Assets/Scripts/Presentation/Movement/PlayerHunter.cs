using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHunter : MonoBehaviour, IPlayer
{
    [Inject] private IControlCharacter _controller;
    [Inject] private IGameUI _gameUI;
    [Inject] private IEventBus _eventBus;
    [Inject] private IVibration _vibro;
    private Hunter _hunter;

    public Vector3 GetPosition() => transform.position;

    private void Start()
    {
        _hunter = GetComponent<Hunter>();
        if (_controller != null)
        {
            _controller.SetOnActionClicked(() =>
            {
                _hunter.Boost();
            });
            _controller.SetOnDebafClicked(() =>
            {
                _hunter.SpawnDebaff();
            });
            _hunter.SetOnHealthChanged((lifes) =>
            {
                _gameUI.SetLifesValue(lifes);
            });
            _hunter.SetOnBoostStateChanged((state) =>
            {
                //Debug.Log("State changed "+state.ToString());
                //_controller.SetActionEnabled(state);
                if (state==true)
                    _vibro.Play();
                if (state==false)
                    _controller.SetActionCooldown(_hunter.Model.BoostRestartTime);
            });
            _hunter.SetOnHunterModeChanged((state) => { 
                _controller.SetActionEnabled(!state);
                if (state == true)
                    _gameUI.ShowAlertMessage("Attack them all!");
                else
                    _gameUI.CloseAlertMessage();
            });
            _hunter.SetOnDestroying(() =>
            {
                _eventBus?.NotifyObservers(GameEventType.PLAYER_DEAD);
                _controller.SetOnActionClicked(null);
                _controller.Hide();
                _vibro.PlayMillis(500);
            });
        }
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

    /*private void OnDestroy()
    {
        _eventBus?.NotifyObservers(GameEventType.PLAYER_DEAD);
        _controller.SetOnActionClicked(null);
        _controller.Hide();
    }*/

    public int GetLifes()
    {
        return _hunter.Lifes;
    }
}
