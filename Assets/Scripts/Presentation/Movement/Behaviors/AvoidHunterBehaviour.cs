using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AvoidHunterBehaviour : AIBehavior
{
    private const float _dangerDistance = 20;
    private float _fear;
    private Hunter _hunter;

    public AvoidHunterBehaviour(float fear, Hunter hunter)
    {
        _fear = fear;
        _hunter = hunter;
    }

    public override AIBehaviorModel Update()
    {
        Hunter nearest = GetNearestObject<Hunter>(_hunter.transform.position, _hunter);
        if (nearest != null && nearest.Lifes > _hunter.Lifes)
        {
            var distance = Vector3.Distance(nearest.transform.position, _hunter.transform.position);
            if (distance < _dangerDistance)
            {
                var acceleration = distance < _dangerDistance / 2;
                var direction = -(nearest.transform.position - _hunter.transform.position).normalized;
                return new AIBehaviorModel(_fear, new Vector2(direction.x, direction.z), acceleration);
            }
        }

        return new AIBehaviorModel();
    }
}
