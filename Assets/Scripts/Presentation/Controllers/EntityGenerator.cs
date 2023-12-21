using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

public class EntityGenerator : MonoBehaviour
{
    public int StartGenerationCount = 100;
    public int GenerationAreaSize = 10;
    public int MinAmountInField = 90;
    public int MinBoosterCount = 2;
    private System.Random _rnd = new System.Random();
    //public List<EntityType> GenTypes = new List<EntityType>();
    [Inject] private EntityFactory _entityFactory;
    [Inject] private BoosterFactory _boosterFactory;

    private void Start()
    {
        Generate(StartGenerationCount);
        StartCoroutine(CheckingCount());
    }

    private Vector3 GetRandomPlace()
    {
        int middle = GenerationAreaSize / 2;
        return new Vector3(middle - _rnd.Next(GenerationAreaSize), transform.position.y, middle - _rnd.Next(GenerationAreaSize));
    }

    public void Generate(int count)
    {
        var enumsArr = Enum.GetValues(typeof(EntityType)).Cast<EntityType>().ToList();
        var rnd = new System.Random();
        //int middle = GenerationAreaSize / 2;
        for (int i = 0; i < count; i++)
        {

            Vector3 placeVector = GetRandomPlace(); //new Vector3(middle - rnd.Next(GenerationAreaSize), transform.position.y, middle - rnd.Next(GenerationAreaSize));
            var entity = _entityFactory.Spawn(enumsArr[rnd.Next(1,enumsArr.Count)]);
            //entity.transform.parent = this.transform;
            //entity.transform.position = transform.position + Vector3.left * i;
            entity.transform.position = placeVector;
        }

        var boostersArr = Enum.GetValues(typeof(BoosterType)).Cast<BoosterType>().ToList();
        var needBoosters = MinBoosterCount - FindObjectsOfType<Booster>().Length;
        //Debug.Log();
        if (needBoosters > 0)
        {
            Vector3 placeVector = GetRandomPlace(); //new Vector3(middle - rnd.Next(GenerationAreaSize), transform.position.y, middle - rnd.Next(GenerationAreaSize));
            var booster = _boosterFactory.Spawn(boostersArr[rnd.Next(0, boostersArr.Count)]);
            //entity.transform.parent = this.transform;
            //entity.transform.position = transform.position + Vector3.left * i;
            booster.transform.position = placeVector;
        }
        
    }

    private IEnumerator CheckingCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            var allFood = FindObjectsOfType<Entity>().ToList();
            if (allFood.Count < MinAmountInField)
            {
                //Generate(StartGenerationCount - MinAmountInField);
                StartCoroutine(GenCoroutine(StartGenerationCount - MinAmountInField, 10));
            }
        }
    }

    private IEnumerator GenCoroutine(int count, int sectionCount)
    {
        int genLeft = count;
        while(genLeft>0)
        {
            int countToGen = Math.Min(sectionCount, genLeft);
            Generate(countToGen);
            genLeft = genLeft - countToGen;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
