using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIHunter : MonoBehaviour
{
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
        resDirection = FoodCollectLayer();


        return resDirection;
    }

    private Vector2 FoodCollectLayer()
    {
        //סכמי סבמנא זנאעגû
        float currentDist = 1000000.0f;
        Entity nearest = null;

        var allFood = Resources.FindObjectsOfTypeAll(typeof(Entity)) as Entity[];        

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
            

            if (nearest.transform.parent == null)
            {
                UnityEngine.Debug.Log("Find Problem Food");
                UnityEngine.Debug.Log(nearest.gameObject.transform.position);
                UnityEngine.Debug.Log(nearest.gameObject.name);
            }

            
            //Debug.Log("cross " + between.ToString());            
            return new Vector2(between.x, between.z);
        }
        return Vector2.zero;
    }

}
