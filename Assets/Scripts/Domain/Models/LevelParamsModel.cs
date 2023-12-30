using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelParamsModel
{
    public List<MobModel> Mobs = new List<MobModel>();
    public int FoodCount = 0;

    public LevelParamsModel(List<MobModel> mobs, int foodCount)
    {
        Mobs = mobs;
        FoodCount = foodCount;
    }
}
