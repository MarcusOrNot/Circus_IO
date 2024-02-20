using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICloudGameStats
{
    public void GetStat(GameStatsType type, Action<bool, int> onResult);
    public void SetGameStat(GameStatsType type, int value, Action<bool> onResult);
}