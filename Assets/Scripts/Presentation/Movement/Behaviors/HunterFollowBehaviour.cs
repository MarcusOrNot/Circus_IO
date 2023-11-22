using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class HunterFollowBehaviour : AIBehavior
{
    private float _agressive;
    private Hunter _hunter;

    public HunterFollowBehaviour(float agressive, Hunter sourceHunter)
    {
        _agressive = agressive;
        _hunter = sourceHunter;
    }

    public override AIBehaviorModel Update()
    {
        Hunter nearest = GetNearestObject<Hunter>(_hunter.transform.position, _hunter);
        if (nearest != null && nearest.Lifes<_hunter.Lifes)
        {
            //Debug.Log("Now hunter should go "+_hunter.name);
            var between = (nearest.transform.position - _hunter.transform.position).normalized;
            //Debug.Log("cross " + between.ToString());            
            //return new Vector2(between.x, between.z);
            return new AIBehaviorModel(1, new Vector2(between.x, between.z));
        }
        return new AIBehaviorModel();
    }
}
