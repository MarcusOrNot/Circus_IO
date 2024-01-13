using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatService
{
    private const int COMPLETE_BONUS = 35;
    private const int LOSE_BONUS = 0;
    private IGameStats _gameStats;
    public GameStatService(IGameStats gameStats)
    {
        _gameStats = gameStats;
    }

    public static int GetLevel(int exp)
    {
        //return Mathf.FloorToInt(Mathf.Log10(exp/10+1)*10);
        return Mathf.FloorToInt(exp/75.0f);
    }
    public static int GetNeedExpByLevel(int level)
    {
        //return (10 ^ (level / 10) - 1) * 10;
        return Mathf.FloorToInt(level * 75);
    }
    public int GetCurrentLevel()
    {
        return GetLevel(_gameStats.GetStat(GameStatsType.EXP));
        //Mathf.FloorToInt();
    }
    public int GetCurrentExp()
    {
        return _gameStats.GetStat(GameStatsType.EXP);
    }
    public static int CalculateWinnerCoins(int lifes, int enemyEaten)
    {
        return COMPLETE_BONUS+lifes/10+enemyEaten*10;
    }
    public static int CalculateLoseCoins(int timeLived, int enemyEaten)
    {
        return LOSE_BONUS+ timeLived/5 + enemyEaten*5;
    }
}
