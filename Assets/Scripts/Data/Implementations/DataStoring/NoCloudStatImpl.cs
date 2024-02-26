using System;
using System.Collections.Generic;

public class NoCloudStatImpl : ICloudGameStats
{
    public void GetGameData(List<string> data, Action<bool, Dictionary<string, string>> onResult)
    {
        onResult?.Invoke(false, new Dictionary<string, string>());
    }

    public void GetStats(List<GameStatsType> stats, Action<bool, Dictionary<GameStatsType, int>> onResult)
    {
        onResult?.Invoke(false, new Dictionary<GameStatsType, int>());
    }

    public void SetGameData(Dictionary<string, string> data, Action<bool> onResult)
    {
        onResult?.Invoke(false);
    }

    public void SetGameStat(GameStatsType type, int value, Action<bool> onResult)
    {
        onResult?.Invoke(false);
    }

    public void SetGameStats(Dictionary<GameStatsType, int> values, Action<bool> onResult)
    {
        onResult?.Invoke(false);
    }
}
