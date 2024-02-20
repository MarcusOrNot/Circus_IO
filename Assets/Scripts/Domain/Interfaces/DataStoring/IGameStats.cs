using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStats
{
    public int GetStat(GameStatsType type);
    public void SetGameStat(GameStatsType type, int value);
    //public void ChangeGameStat(GameStatsType type, int value);
    //public void SetOnStatChanged(IStatsObserver observer);
    //public void RemoveOnSettingChanged(IStatsObserver observer);
    //public void NotifyObservers(GameStatsType stat);
}
