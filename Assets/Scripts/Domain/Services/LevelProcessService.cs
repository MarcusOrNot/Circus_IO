using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelProcessService
{
    private const int MAX_MOBS = 10;
    private const int MAX_AREA = 300;
    private const int MAX_FOOD = 1000;
    private const int MAX_BOOSTERS = 5;
    private const int MAX_LIFES_MOBS = 1000;

    private List<Hunter> _allHunters;
    public LevelProcessService(List<Hunter> hunters)
    {
        _allHunters = hunters;
    }

    public LevelParamsModel GenerateLevel(int difficulty) 
    {
        int sumLifes = difficulty * 10;
        sumLifes = Mathf.Min( Random.Range(sumLifes/2, sumLifes), MAX_LIFES_MOBS);

        List<MobModel> mobs = new List<MobModel>();
        int mobsCount = 1 + difficulty/2;
        mobsCount = Mathf.Min(mobsCount, MAX_MOBS);
        var currentHunters = GetHunterModels(_allHunters);
        while (mobsCount > 0 && currentHunters.Count > 0)
        {
            var hunter = currentHunters[Random.Range(0, currentHunters.Count - 1)];
            mobs.Add(new MobModel(hunter.HunterType, HatType.CAP, Random.Range(0, sumLifes)));

            currentHunters.Remove(hunter);
            mobsCount--;
        }

        int foodCount = 50 + difficulty * 10;
        int boostersCount = difficulty / 3;
        int areaSize = 100 + difficulty * 10;

        return new LevelParamsModel(
            mobs, 
            Mathf.Min(foodCount, MAX_FOOD), 
            Mathf.Min(boostersCount, MAX_BOOSTERS),
            Mathf.Min(areaSize, MAX_AREA), 
            30
            );
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
