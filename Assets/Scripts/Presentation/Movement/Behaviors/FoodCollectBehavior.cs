using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollectBehavior : AIBehavior
{
    private float _appetite;
    private Transform _transform;
    public FoodCollectBehavior(float appetite, Transform sourceTransform)
    {
        _appetite = appetite;
        _transform = sourceTransform;
    }

    public override AIBehaviorModel Update()
    {
        //סכמי סבמנא זנאעגû
        Entity nearest = GetNearestObject<Entity>(_transform.position);
        if (nearest != null)
        {
            var between = (nearest.transform.position - _transform.position).normalized;
            //Debug.Log("cross " + between.ToString());            
            //_direction = new Vector2(between.x, between.z);
            return new AIBehaviorModel(0.5f, new Vector2(between.x, between.z));
        }


        return new AIBehaviorModel();
    }
}
