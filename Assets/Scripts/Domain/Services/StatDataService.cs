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
        var dataParamsList = Utils.GetListOfEnums<GameStatsType>();
        _cloudStats.GetStats(dataParamsList, (isSuccess, values) =>
        {
            SetLocalGameStats(values);
        });

        onInit?.Invoke(true);
    }

    public int GetStat(GameStatsType type) => _gameStats.GetStat(type);

    public void SetGameStat(GameStatsType type, int value)
    {
        _gameStats.SetGameStat(type, value);
        NotifyObservers(type);
        _cloudStats.SetGameStat(type, value, (success) => {});
    }

    public void SetGameStats(Dictionary<GameStatsType, int> values)
    {
        SetLocalGameStats(values);
        _cloudStats.SetGameStats(values, (success) => {});
    }

    private void SetLocalGameStats(Dictionary<GameStatsType, int> values)
    {
        _gameStats.SetGameStats(values);
        foreach (KeyValuePair<GameStatsType, int> value in values)
            NotifyObservers(value.Key);
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

    public void ChangeGameStat(GameStatsType type, int delta)
    {
        SetGameStat(type, _gameStats.GetStat(type) + delta);
    }

    public void ChangeGameStats(Dictionary<GameStatsType, int> deltas)
    {
        Dictionary<GameStatsType, int> newValues = new Dictionary<GameStatsType, int>();
        foreach (KeyValuePair<GameStatsType, int> delta in deltas)
            newValues.Add(delta.Key, _gameStats.GetStat(delta.Key)+delta.Value);
        SetGameStats(newValues);
    }
}
