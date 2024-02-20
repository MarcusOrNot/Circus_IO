using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDataService
{
    private IGameStats _gameStats;
    private ICloudGameStats _cloudStats;
    private List<IStatsObserver> _statListeners = new List<IStatsObserver>();
    public StatDataService(IGameStats gameStats, ICloudGameStats cloudStats)
    {
        _gameStats = gameStats;
        _cloudStats = cloudStats;
    }

    public void InitStartData(Action<bool> onInit)
    {
        _cloudStats.GetStat(GameStatsType.COINS, (success, res) =>
        {
            Debug.Log("Now is "+success.ToString()+"res is "+res.ToString());
        });
        onInit?.Invoke(true);
    }

    public int GetStat(GameStatsType type) => _gameStats.GetStat(type);

    public void SetGameStat(GameStatsType type, int value)
    {
        _gameStats.SetGameStat(type, value);
        NotifyObservers(type);
        _cloudStats.SetGameStat(type, value, (success) =>
        {

        });
    }


    private void NotifyObservers(GameStatsType stat)
    {
        foreach (var observer in _statListeners)
        {
            observer.Notify(stat);
        }
    }

    public void RemoveOnSettingChanged(IStatsObserver observer)
    {
        _statListeners.Remove(observer);
    }

    public void SetOnStatChanged(IStatsObserver observer)
    {
        if (_statListeners.Contains(observer) == false)
        {
            _statListeners.Add(observer);
        }
    }

    public void ChangeGameStat(GameStatsType type, int value)
    {
        SetGameStat(type, _gameStats.GetStat(type) + value);
    }
}
