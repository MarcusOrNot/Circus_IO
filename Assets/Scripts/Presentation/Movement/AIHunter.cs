using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIHunter : MonoBehaviour
{
    [SerializeField] private int _warningAvoidDistance= 15;
    //[SerializeField] private int _aggressionDistance = 10;
    [Range(0, 1)] public float Agressive = 0.6f;
    [Range(0, 1)] public float Appetite = 0.5f;
    private Hunter _hunter;
    private List<AIBehavior> _behaviors = new List<AIBehavior>();
    private Vector2 _aiDirection = Vector2.zero;
    private void Awake()
    {
        _hunter = GetComponent<Hunter>();
        _hunter.SetOnHealthChanged((health) => { RefreshDirection(); });
    }
    // Start is called before the first frame update
    void Start()
    {
        //Собираем поведение
        _behaviors.Add(new HunterFollowBehaviour(Agressive, _hunter));
        _behaviors.Add(new FoodCollectBehavior(Appetite, transform));

        StartCoroutine(AIControlCoroutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        _hunter.Move(_aiDirection);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator AIControlCoroutine()
    {
        while (true)
        {
            _aiDirection = GetResultVector();
            yield return new WaitForSeconds(0.5f);
            //_aiDirection = RefreshDirection();
        }
    }

    private Vector2 GetResultVector()
    {
        Vector2 result = Vector2.zero;
        float weight = 0;
        foreach (AIBehavior behavior in _behaviors)
        {
            var behaviorModel = behavior.Update();
            if (behaviorModel.Weight>weight)
            {
                result = behaviorModel.Direction;
                weight = behaviorModel.Weight;
            }
        }

        return result;
    }

    private Vector2 RefreshDirection()
    {
        Vector2 resDirection = Vector2.zero;

        //resDirection = new Vector2(1,1);
        resDirection = AvoidHunterLayer();
        if (resDirection == Vector2.zero)
            resDirection = HunterFollowLayer();
        if (resDirection == Vector2.zero)
            resDirection = FoodCollectLayer();
        //resDirection = HunterFollowLayer();
        
        //Debug.Log(resDirection);


        return resDirection;
    }

    private Vector2 FoodCollectLayer()
    {
        //слой сбора жратвы
        float currentDist = 1000000.0f;
        Entity nearest = null;

        //var allFood = GameObject.FindObjectsOfTypeAll(typeof(Entity)) as Entity[];
        var allFood = FindObjectsOfType<Entity>();

        foreach (var food in allFood)
        {
            var dist = Vector3.Distance(food.transform.position, transform.position);
            if (dist < currentDist)
            {
                nearest = food;
                currentDist = dist;
            }
        }
        if (nearest != null)
        {
            var between = (nearest.transform.position - transform.position).normalized;            
            //Debug.Log("cross " + between.ToString());            
            return new Vector2(between.x, between.z);
        }
        return Vector2.zero;
    }

    private Vector2 HunterFollowLayer()
    {
        float currentDist = 1000000.0f;
        Hunter nearest = null;

        //var allFood = GameObject.FindObjectsOfTypeAll(typeof(Entity)) as Entity[];
        var allHunters = FindObjectsOfType<Hunter>().ToList();
        allHunters.Remove(_hunter);

        foreach (var hunter in allHunters)
        {
            var dist = Vector3.Distance(hunter.transform.position, transform.position);
            if (dist < currentDist && _hunter.Lifes> hunter.Lifes)
            {
                nearest = hunter;
                currentDist = dist;
            }
        }
        if (nearest != null)
        {
            //Debug.Log("Now hunter should go "+_hunter.name);
            var between = (nearest.transform.position - transform.position).normalized;
            //Debug.Log("cross " + between.ToString());            
            return new Vector2(between.x, between.z);
        }
        return Vector2.zero;
    }

    private Vector2 AvoidHunterLayer()
    {
        float currentDist = 1000000.0f;
        Hunter nearest = null;

        //var allFood = GameObject.FindObjectsOfTypeAll(typeof(Entity)) as Entity[];
        var allHunters = FindObjectsOfType<Hunter>().ToList();
        allHunters.Remove(_hunter);

        foreach (var hunter in allHunters)
        {
            var dist = Vector3.Distance(hunter.transform.position, transform.position);
            if (dist < currentDist && dist < _warningAvoidDistance && _hunter.Lifes < hunter.Lifes)
            {
                nearest = hunter;
                currentDist = dist;
            }
        }
        if (nearest != null)
        {
            var between = -(nearest.transform.position - transform.position).normalized;
            //Debug.Log("cross " + between.ToString());            
            return new Vector2(between.x, between.z);
        }
        return Vector2.zero;
    }

}
