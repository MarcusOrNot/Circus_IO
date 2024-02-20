using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YandexCloudStatsImpl : ICloudGameStats
{
    private Action<bool, int> _onGettingStat;
    private Action<bool> _onSendingStat;
    public YandexCloudStatsImpl()
    {
        YandexSDK.instance.onGameStatReceived += Instance_onGameStatReceived;
    }

    private void Instance_onGameStatReceived(string obj)
    {
        Debug.Log("Server result is "+obj);
        var res = JsonUtility.FromJson<string[]>(obj);
        if (res.Length>0)
        {
            //_onGettingStat?.Invoke(true, );
            _onGettingStat?.Invoke(false, 0);
        }
        else
        {
            _onGettingStat?.Invoke(false, 0);
        }
    }

    public void GetStat(GameStatsType type, Action<bool, int> onResult)
    {
        _onGettingStat = onResult;
        string[] sending = new string[1];
        sending[0] = type.ToString();
        YandexSDK.instance.RequestStatData(sending);
        //onResult.Invoke(true, 100);
    }

    public void SetGameStat(GameStatsType type, int value, Action<bool> onResult)
    {
        _onSendingStat = onResult;
        //onResult.Invoke(true);
        
        //YandexSDK.instance.SetStat();
    }
}
