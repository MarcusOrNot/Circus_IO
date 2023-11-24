using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDamageZoneBehaviour : AIBehavior
{
    private float _fear;
    private Hunter _hunter;
    //private DamageZoneConroller _damageZone;
    public AvoidDamageZoneBehaviour(float fear, Hunter hunter)
    {
        _fear = fear;
        _hunter = hunter;
        //_damageZone = damageZone;
    }

    public override AIBehaviorModel Update()
    {
        if (DamageZone==null) return new AIBehaviorModel();
        var direction = (DamageZone.transform.position - _hunter.transform.position).normalized;
        if (IsDangerZone(_hunter.transform.position-direction*5))
        {
            return new AIBehaviorModel(_fear, new Vector2(direction.x, direction.z), false);
        }
        return new AIBehaviorModel();
    }
}
