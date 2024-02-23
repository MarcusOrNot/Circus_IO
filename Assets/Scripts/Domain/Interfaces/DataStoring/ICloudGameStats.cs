using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICloudGameStats
{
    public void GetStats(List<GameStatsType> stats, Action<bool, Dictionary<GameStatsType, int>> onResult);
    public void SetGameStat(GameStatsType type, int value, Action<bool> onResult);
    public void SetGameStats(Dictionary<GameStatsType, int> values, Action<bool> onResult);
    public void GetGameData(List<string> dataParams, Action<bool, Dictionary<string, string>> onResult);
    public void SetGameData(Dictionary<string, string> data, Action<bool> onResult);
}