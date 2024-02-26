#if UNITY_WEBGL || UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YandexCloudStatsImpl : ICloudGameStats
{
    private Action<bool, Dictionary<GameStatsType, int>> _onGettingStats;
    private Action<bool> _onSendingStat;
    private Action<bool, Dictionary<string, string>> _onGameDataRecieved;
    private Action<bool> _onSendingData;

    public YandexCloudStatsImpl()
    {
        //Debug.Log("Now inited yandex cloudStat Impl");
        //YandexSDK.instance.onGameStatReceived = Instance_onGameStatReceived;
        YandexSDK.instance.onGameStatReceived = (Dictionary<string, int> values) =>
        {
            var res = new Dictionary<GameStatsType, int>();
            foreach (var kvp in values)
            {
                //res.Add((GameStatsType)Enum.Parse(typeof(GameStatsType), kvp.Key, true), kvp.Value);
                res.Add(Utils.GetEnumByString<GameStatsType>(kvp.Key), kvp.Value);
            }

            _onGettingStats?.Invoke(true, res);
        };

        YandexSDK.instance.onGameDataReceived = (data) =>
        {
            _onGameDataRecieved?.Invoke(true, data);
        };
    }

    /*private void Instance_onGameStatReceived(Dictionary<string, int> values)
    {
        Debug.Log("Values count is "+values.Count.ToString());
        var res = new Dictionary<GameStatsType, int>();
        foreach (var kvp in values)
        {
            res.Add((GameStatsType)Enum.Parse(typeof(GameStatsType), kvp.Key, true), kvp.Value);
        }

        _onGettingStats?.Invoke(true, res);
    }*/

    /*private void Instance_onGameStatReceived(string obj)
    {
        Debug.Log("Server result is "+obj);
        _onGettingStat?.Invoke(false,0);
    }*/

    /*public void GetStat(GameStatsType type, Action<bool, int> onResult)
    {
        _onGettingStat = onResult;
        string[] sending = new string[1];
        sending[0] = type.ToString();
        YandexSDK.instance.RequestStatData(sending);
        //onResult.Invoke(true, 100);
    }*/

    public void SetGameStat(GameStatsType type, int value, Action<bool> onResult)
    {
        Dictionary<String, int> values = new Dictionary<String, int>()
        {
            {type.ToString(), value }
        };
        YandexSDK.instance.SetStat(values);
        onResult?.Invoke(true);
    }

    public void SetGameStats(Dictionary<GameStatsType, int> values, Action<bool> onResult)
    {
        //throw new NotImplementedException();
        var resValues = new Dictionary<string, int>();
        foreach(var kvp in values)
        {
            resValues.Add(kvp.Key.ToString(), kvp.Value);
        }
        YandexSDK.instance.SetStat(resValues);
        onResult?.Invoke(true);
    }

    public void GetStats(List<GameStatsType> stats, Action<bool, Dictionary<GameStatsType, int>> onResult)
    {
        _onGettingStats = onResult;
        var statStrings = new List<string>();
        foreach (var item in stats)
        {
            statStrings.Add(item.ToString());
        }
        YandexSDK.instance.RequestStatData(statStrings.ToArray());
    }

    public void GetGameData(List<string> dataParams, Action<bool, Dictionary<string, string>> onResult)
    {
        _onGameDataRecieved = onResult;
        YandexSDK.instance.RequestGameData(dataParams.ToArray());
    }

    public void SetGameData(Dictionary<string, string> data, Action<bool> onResult)
    {
        YandexSDK.instance.SendGameData(data);
        onResult.Invoke(true);
    }
}

#endif