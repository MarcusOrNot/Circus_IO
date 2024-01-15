using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatService: IGameEventObserver
{
    private IEventBus _eventBus;
    private int _huntersEaten = 0;
    private int _maxPlayerHealth = 0;
    //private long _startedTime = 0;
    private DateTime _startedDate;
    public LevelStatService(IEventBus eventBus)
    {
        _eventBus = eventBus;
        //_startedTime = DateTime.Now.Ticks;
        _startedDate = DateTime.Now;
        RegisterService();
    }

    private void RegisterService()
    {
        _eventBus.RegisterObserver(this);
    }

    public void UnregisterService()
    {
        _eventBus.RemoveObserver(this);
        //Debug.Log("Now end level "+SecondsElapsed.ToString());
    }

    public void Notify(GameEventType gameEvent)
    {
        if (gameEvent==GameEventType.PLAYER_HEALTH_CHANGED)
        {
            var player = Level.Instance.GetPlayer();
            if (player!=null && player.GetLifes()>_maxPlayerHealth)
                _maxPlayerHealth=player.GetLifes();
        }
        if (gameEvent == GameEventType.PLAYER_ATE_HUNTER)
            _huntersEaten++;
    }

    public int HuntersEaten => _huntersEaten;
    public int MaxPlayerHealth => _maxPlayerHealth;
    public int SecondsElapsed
    {
        get
        {
            //Debug.Log(DateTime.Now.Ticks.ToString());
            //return (int) ((DateTime.Now.Ticks - _startedTime)/1000);
            TimeSpan elapsedTime = DateTime.Now - _startedDate;
            return elapsedTime.Seconds;
        }
    }

    /*public int GetExp()
    {
        return 100;
    }*/
}
