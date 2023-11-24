using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class EntityGenerator : MonoBehaviour
{
    public int StartGenerationCount = 100;
    public int GenerationAreaSize = 10;
    public int MinAmountInField = 90;
    //public List<EntityType> GenTypes = new List<EntityType>();
    [Inject] private EntityFactory _entityFactory;

    private void Start()
    {
        Generate(StartGenerationCount);
        StartCoroutine(CheckingCount());
    }

    public void Generate(int count)
    {
        var enumsArr = Enum.GetValues(typeof(EntityType)).Cast<EntityType>().ToList();
        var rnd = new System.Random();
        int middle = GenerationAreaSize / 2;
        for (int i = 0; i < count; i++)
        {
            
            Vector3 placeVector = new Vector3(middle - rnd.Next(GenerationAreaSize), transform.position.y, middle - rnd.Next(GenerationAreaSize));
            var entity = _entityFactory.Spawn(enumsArr[rnd.Next(1,enumsArr.Count)]);
            //entity.transform.parent = this.transform;
            //entity.transform.position = transform.position + Vector3.left * i;
            entity.transform.position = placeVector;
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
                Generate(StartGenerationCount - MinAmountInField);
            }
        }
    }
}
