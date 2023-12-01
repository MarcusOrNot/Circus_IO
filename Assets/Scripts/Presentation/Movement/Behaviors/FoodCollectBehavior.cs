using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollectBehavior : AIBehavior
{
    private float _appetite;
    private Transform _transform;
    private Hunter _hunter;
    //private DamageZoneConroller _damageZone;
    public FoodCollectBehavior(float appetite, Transform sourceTransform, Hunter hunter)
    {
        _appetite = appetite;
        _transform = sourceTransform;
        //_damageZone = damageZone;
        _hunter = hunter;
    }

    public override AIBehaviorModel Update()
    {
        //סכמי סבמנא זנאעגû
        Entity nearest = GetNearestObject<Entity>(_transform.position, null);
        
        if (nearest != null && _hunter.KaufmoIsActive == false)
        {
            var between = (nearest.transform.position - _transform.position).normalized;
            //Debug.Log("cross " + between.ToString());            
            //_direction = new Vector2(between.x, between.z);
            return new AIBehaviorModel(_appetite, new Vector2(between.x, between.z), false);
        }


        return new AIBehaviorModel();
    }
}
