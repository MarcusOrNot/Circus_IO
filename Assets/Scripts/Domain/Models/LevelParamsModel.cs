using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelParamsModel
{
    public List<MobModel> Mobs = new List<MobModel>();
    public int FoodCount = 0;
    public int BoostersCount = 0;
    public int MaxZoneSize = 0;
    public int ZoneChangeTime = 0;

    public LevelParamsModel(List<MobModel> mobs, int foodCount, int boostersCount, int maxZoneSize, int zoneChangeTime)
    {
        Mobs = mobs;
        FoodCount = foodCount;
        BoostersCount = boostersCount;
        MaxZoneSize = maxZoneSize;
        ZoneChangeTime = zoneChangeTime;
    }

    public override string ToString()
    {
        //return base.ToString();
        return "Mobs Count = "+Mobs.Count.ToString()+", Food count "+FoodCount.ToString()
            +", Boosters Count "+BoostersCount.ToString()
            +", Max Zone "+MaxZoneSize.ToString()+", Zone Change "+ZoneChangeTime.ToString();
    }
}
