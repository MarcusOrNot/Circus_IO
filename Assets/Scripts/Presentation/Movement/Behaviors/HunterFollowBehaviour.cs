using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HunterFollowBehaviour : AIBehavior
{
    private float _aggressionDistance = 20;
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
            if (nearest.KaufmoIsActive == true && _hunter.KaufmoIsActive==false) return new AIBehaviorModel();
            var distance = Vector3.Distance(nearest.transform.position, _hunter.transform.position);
            if (distance < _aggressionDistance*(Mathf.Min(_hunter.Lifes/ nearest.Lifes, 2)) || _hunter.KaufmoIsActive == true)
            {
                //Debug.Log("Now hunter should go "+_hunter.name);
                var between = (nearest.transform.position - _hunter.transform.position).normalized;
                var accelerate = Vector3.Angle(between, _hunter.transform.forward) < 10;
                //Debug.Log("cross " + between.ToString());            
                //return new Vector2(between.x, between.z);
                return new AIBehaviorModel(_agressive, new Vector2(between.x, between.z), accelerate);
            }
            else
            {
                var between = (nearest.transform.position - _hunter.transform.position).normalized;
                return new AIBehaviorModel(_agressive/10, new Vector2(between.x, between.z), false);
            }
        }
        return new AIBehaviorModel();
    }
}
