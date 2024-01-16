using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelParamsModel
{
    public List<MobModel> Mobs = new List<MobModel>();
    public int FoodCount = 0;
    public int MaxZoneSize = 0;
    public int ZoneChangeTime = 0;

    public LevelParamsModel(List<MobModel> mobs, int foodCount, int maxZoneSize, int zoneChangeTime)
    {
        Mobs = mobs;
        FoodCount = foodCount;
        MaxZoneSize = 0;
        ZoneChangeTime = 0;
    }
}
