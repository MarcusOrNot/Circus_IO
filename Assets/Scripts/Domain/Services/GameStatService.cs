using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatService
{
    private const int COMPLETE_BONUS = 25;
    private const int LOSE_BONUS = 0;
    private const float EXP_MULTIPLIER = 3.0f;
    private const int INIT_EXP = 100;
    private const float PROGRESS_STEP = 1.5f;
    private IGameStats _gameStats;
    public GameStatService(IGameStats gameStats)
    {
        _gameStats = gameStats;
    }

    public static int GetLevel(float exp)
    {
        /*int level = 0;
        while(exp>0)
        {
            level++;
            exp=exp - GetNeedExpByLevel(level);
        }
        return level;*/
        if (exp <= INIT_EXP)
        {
            return 1;
        }
        else
        {
            return Mathf.FloorToInt(Mathf.Log(exp / INIT_EXP, PROGRESS_STEP)) + 1;
        }
    }
    public static float GetNeedExpByLevel(int level)
    {
        //return (10 ^ (level / 10) - 1) * 10;
        //return Mathf.FloorToInt(level * 75);
        //return 100 + 20 * level;
        //return INIT_EXP + (level - 1) * PROGRESS_STEP;
        if (level <= 1)
        {
            return INIT_EXP;
        }
        else
        {
            return Mathf.FloorToInt(INIT_EXP * Mathf.Pow(PROGRESS_STEP, level - 1));
        }
    }
    /*public static int GetTargetExp(int level)
    {
        int sum = 0;
        for (int i=0; i<level;i++)
        {
            sum += GetNeedExpByLevel(level);
        }
        return sum;
    }*/
    public int GetCurrentLevel()
    {
        return GetLevel(_gameStats.GetStat(GameStatsType.EXP));
        //Mathf.FloorToInt();
        //return _gameStats.GetStat(GameStatsType.LEVEL+1);
    }
    public int GetCurrentExp()
    {
        return _gameStats.GetStat(GameStatsType.EXP);
    }
    public static int CalculateWinnerCoins(int lifes, int enemyEaten)
    {
        return COMPLETE_BONUS+lifes/15+enemyEaten*7;
    }
    public static int CalculateLoseCoins(int timeLived, int enemyEaten, int maxHealth)
    {
        return LOSE_BONUS+ timeLived/5 + enemyEaten*5 + maxHealth/30;
    }
    public static int GetExpFromCoins(int coins)
    {
        return Mathf.FloorToInt(coins*EXP_MULTIPLIER);
    }
    /*public static int GetWinnerExp(int lifes, int enemyEaten)
    {
        return UnityEngine.Mathf.FloorToInt ((COMPLETE_BONUS + lifes / 10 + enemyEaten * 7)* EXP_MULTIPLIER);
    }*/
}
