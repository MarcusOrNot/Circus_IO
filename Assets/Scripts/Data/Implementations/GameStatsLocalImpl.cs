using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsLocalImpl : IGameStats
{
    /*private const string STAT_OXYGEN = "stat_oxigen";
    private const string STAT_WATER = "stat_water";
    private const string STAT_VEGETABLES = "stat_vegetables";
    private const string STAT_MONEY = "stat_money";

    public int Oxygen { get => PlayerPrefs.GetInt(STAT_OXYGEN, 0); set => PlayerPrefs.SetInt(STAT_OXYGEN, value); }
    public int Water { get => PlayerPrefs.GetInt(STAT_WATER, 0); set => PlayerPrefs.SetInt(STAT_WATER, value); }
    public int Vegetables { get => PlayerPrefs.GetInt(STAT_VEGETABLES, 0); set => PlayerPrefs.SetInt(STAT_VEGETABLES, value); }
    public int Money { get => PlayerPrefs.GetInt(STAT_MONEY, 0); set => PlayerPrefs.SetInt(STAT_MONEY, value); }*/
    public int GetStat(GameStatsType type)
    {
        return PlayerPrefs.GetInt(GetStatStringByEnum(type), 0);
    }

    public void SetGameStat(GameStatsType type, int value)
    {
        PlayerPrefs.SetInt(GetStatStringByEnum(type), value);
    }

    private string GetStatStringByEnum(GameStatsType gameStat)
    {
        string res = "stat_game_default";
        switch (gameStat)
        {
            case GameStatsType.SCORE:
                res = "stat_score";
                break;
        }
        return res;
    }
}
