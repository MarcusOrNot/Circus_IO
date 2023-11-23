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
        Hunter nearest = GetNearestObject<Hunter>(_hunter.transform.position, _hunter, null);
        if (nearest != null && nearest.Lifes<_hunter.Lifes)
        {
            //Debug.Log("Now hunter should go "+_hunter.name);
            var between = (nearest.transform.position - _hunter.transform.position).normalized;
            var accelerate = Vector3.Angle(between, _hunter.transform.forward)<10;
            //Debug.Log("cross " + between.ToString());            
            //return new Vector2(between.x, between.z);
            return new AIBehaviorModel(_agressive, new Vector2(between.x, between.z), accelerate);
        }
        return new AIBehaviorModel();
    }
}
