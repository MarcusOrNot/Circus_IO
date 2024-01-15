using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatService
{
    private const int COMPLETE_BONUS = 35;
    private const int LOSE_BONUS = 0;
    private const float EXP_MULTIPLIER = 4.0f;
    private IGameStats _gameStats;
    public GameStatService(IGameStats gameStats)
    {
        _gameStats = gameStats;
    }

    public static int GetLevel(int exp)
    {
        //return Mathf.FloorToInt(Mathf.Log10(exp/10+1)*10);
        //return Mathf.FloorToInt(exp/75.0f);
        //return Mathf.FloorToInt(((float) exp-50)/10);
        int level = 0;
        while(exp>0)
        {
            level++;
            exp=exp - GetNeedExpByLevel(level);
        }
        return level;
    }
    public static int GetNeedExpByLevel(int level)
    {
        //return (10 ^ (level / 10) - 1) * 10;
        //return Mathf.FloorToInt(level * 75);
        return 100 + 20 * level;
    }
    public static int GetTargetExp(int level)
    {
        int sum = 0;
        for (int i=0; i<level;i++)
        {
            sum += GetNeedExpByLevel(level);
        }
        return sum;
    }
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
        return COMPLETE_BONUS+lifes/10+enemyEaten*10;
    }
    public static int CalculateLoseCoins(int timeLived, int enemyEaten, int maxHealth)
    {
        return LOSE_BONUS+ timeLived/5 + enemyEaten*5 + maxHealth/30;
    }
    public static int GetExpFromCoins(int coins)
    {
        return Mathf.FloorToInt(coins*EXP_MULTIPLIER);
    }
}
