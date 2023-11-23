using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AvoidHunterBehaviour : AIBehavior
{
    private float _fear;
    private Hunter _hunter;

    public AvoidHunterBehaviour(float fear, Hunter hunter)
    {
        _fear = fear;
        _hunter = hunter;
    }

    public override AIBehaviorModel Update()
    {
        Hunter nearest = GetNearestObject<Hunter>(_hunter.transform.position, _hunter, null);
        if (nearest != null && nearest.Lifes < _hunter.Lifes)
        {
            var between = -(nearest.transform.position - _hunter.transform.position).normalized;
            return new AIBehaviorModel(_fear, new Vector2(between.x, between.z), true);
        }

        return new AIBehaviorModel();
    }
}
