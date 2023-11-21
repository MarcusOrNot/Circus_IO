using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIHunter : MonoBehaviour
{
    [SerializeField] private int _warningAvoidDistance= 15;
    private Hunter _hunter;
    private Vector2 _aiDirection = Vector2.zero;
    private void Awake()
    {
        _hunter = GetComponent<Hunter>();
        _hunter.SetOnHealthChanged((health) => { RefreshDirection(); });
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AIControlCoroutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        _hunter.Move(_aiDirection);
    }

    private IEnumerator AIControlCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _aiDirection = RefreshDirection();
        }
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
        //סכמי סבמנא זנאעגû
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
