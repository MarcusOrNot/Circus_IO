using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelProcessService
{
    private List<Hunter> _allHunters;
    public LevelProcessService(List<Hunter> hunters)
    {
        _allHunters = hunters;
    }

    public LevelParamsModel GenerateLevel(int difficulty) 
    {
        List<MobModel> mobs = new List<MobModel>();
        int sumLifes = difficulty * 10;
        int mobsCount = Random.Range(5, 10);
        var currentHunters = GetHunterModels(_allHunters);
        while (mobsCount > 0 && currentHunters.Count > 0)
        {
            var hunter = currentHunters[Random.Range(0, currentHunters.Count - 1)];
            mobs.Add(new MobModel(hunter.HunterType, HatType.CAP, UnityEngine.Random.Range(0, sumLifes)));

            currentHunters.Remove(hunter);
            mobsCount--;
        }

        int foodCount = 20 + difficulty * 20;
        int areaSize = 100 + difficulty * 10;

        return new LevelParamsModel(mobs, foodCount, areaSize, 30);
    }

    private static List<HunterModel> GetHunterModels(List<Hunter> hunters)
    {
        List<HunterModel> res = new List<HunterModel>();
        foreach (var item in hunters)
        {
            res.Add(item.Model);
        }
        return res;
    }
}
