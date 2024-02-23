using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsLocalImpl : IGameStats
{
    private List<IStatsObserver> _statListeners = new List<IStatsObserver>();

    public int GetStat(GameStatsType type)
    {
        return PlayerPrefs.GetInt(GetStatStringByEnum(type), 0);
    }

    /*private void NotifyObservers(GameStatsType stat)
    {
        foreach (var observer in _statListeners)
        {
            observer.Notify(stat);
        }
    }

    public void RemoveOnSettingChanged(IStatsObserver observer)
    {
        _statListeners.Remove(observer);
    }*/

    public void SetGameStat(GameStatsType type, int value)
    {
        PlayerPrefs.SetInt(GetStatStringByEnum(type), value);
        //NotifyObservers(type);
    }

    public void SetGameStats(Dictionary<GameStatsType, int> values)
    {
        foreach(KeyValuePair<GameStatsType, int> value in values)
        {
            SetGameStat(value.Key, value.Value);
        }
    }

    /*public void SetOnStatChanged(IStatsObserver observer)
    {
        if (_statListeners.Contains(observer)==false)
        {
            _statListeners.Add(observer);
        }
    }*/

    private string GetStatStringByEnum(GameStatsType gameStat)
    {
        string res = "stat_game_default";
        /*switch (gameStat)
        {
            case GameStatsType.SCORE:
                res = "stat_score";
                break;
        }*/
        res = "stat_"+gameStat.ToString();
        return res;
    }

    /*public void ChangeGameStat(GameStatsType type, int value)
    {
        SetGameStat(type, GetStat(type)+value);
    }*/
}
