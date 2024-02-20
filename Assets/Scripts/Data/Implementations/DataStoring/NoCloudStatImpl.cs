using System;

public class NoCloudStatImpl : ICloudGameStats
{
    public void GetStat(GameStatsType type, Action<bool, int> onResult)
    {
        onResult.Invoke(false, 0);
    }

    public void SetGameStat(GameStatsType type, int value, Action<bool> onResult)
    {
        onResult.Invoke(false);
    }
}
