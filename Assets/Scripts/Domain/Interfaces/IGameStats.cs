using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStats
{
    public int GetStat(GameStatsType type);
    public void SetGameStat(GameStatsType type, int value);
    /*public int Oxygen {  get; set; }
    public int Water { get; set; }
    public int Vegetables { get; set; }
    public int Money { get; set; }*/
    
}
