using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

public class EntityGenerator : MonoBehaviour, IGameEventObserver
{
    [Inject] private ILevelInfo _levelInfo;
    [Inject] private IEventBus _eventBus;
    public int StartGenerationCount = 100;
    public float GenerationAreaSize = 10;
    public int MinAmountInField = 90;
    public int MinBoosterCount = 2;
    //private System.Random _rnd = new System.Random();
    //public List<EntityType> GenTypes = new List<EntityType>();
    [Inject] private EntityFactory _entityFactory;
    [Inject] private BoosterFactory _boosterFactory;

    private void Awake()
    {
        var levelParams = _levelInfo.GetLevelParams();
        if (levelParams!=null)
        {
            StartGenerationCount = levelParams.FoodCount;
            GenerationAreaSize = levelParams.MaxZoneSize;
            MinAmountInField = levelParams.FoodCount / 2;
        }
    }

    private void Start()
    {
        //Debug.Log("Entities zone size = "+GenerationAreaSize.ToString());
        Generate(StartGenerationCount);
        StartCoroutine(CheckingCount());
    }

    private Vector3 GetRandomPlace()
    {
        float middle = GenerationAreaSize / 2;
        return new Vector3(middle - UnityEngine.Random.Range(0, GenerationAreaSize), transform.position.y, middle - UnityEngine.Random.Range(0, GenerationAreaSize));
        //return new Vector3(middle - _rnd.Next(GenerationAreaSize), transform.position.y, middle - _rnd.Next(GenerationAreaSize));
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

    private void OnEnable()
    {
        _eventBus.RegisterObserver(this);
    }

    private void OnDisable()
    {
        _eventBus.RemoveObserver(this);
    }

    public void Notify(GameEventType gameEvent)
    {
        if (gameEvent == GameEventType.ZONE_CHANGED)
        {
            var damageZone = Level.Instance.GetDamageZone();
            if (damageZone != null)
            {
                GenerationAreaSize = damageZone.GetSize();
            }
        }
    }
}
