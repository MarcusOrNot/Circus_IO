using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStats
{
    public int GetStat(GameStatsType type);
    public void SetGameStat(GameStatsType type, int value);
    public string PlayerName { get; set; }
}
