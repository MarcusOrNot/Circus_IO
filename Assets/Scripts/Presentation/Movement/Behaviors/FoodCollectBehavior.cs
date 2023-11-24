using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollectBehavior : AIBehavior
{
    private float _appetite;
    private Transform _transform;
    //private DamageZoneConroller _damageZone;
    public FoodCollectBehavior(float appetite, Transform sourceTransform)
    {
        _appetite = appetite;
        _transform = sourceTransform;
        //_damageZone = damageZone;
    }

    public override AIBehaviorModel Update()
    {
        //סכמי סבמנא זנאעגû
        Entity nearest = GetNearestObject<Entity>(_transform.position, null);
        
        if (nearest != null)
        {
            var between = (nearest.transform.position - _transform.position).normalized;
            //Debug.Log("cross " + between.ToString());            
            //_direction = new Vector2(between.x, between.z);
            return new AIBehaviorModel(_appetite, new Vector2(between.x, between.z), false);
        }


        return new AIBehaviorModel();
    }
}
