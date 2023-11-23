using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDamageZoneBehaviour : AIBehavior
{
    private float _fear;
    private Hunter _hunter;
    private DamageZoneConroller _damageZone;
    public AvoidDamageZoneBehaviour(float fear, Hunter hunter, DamageZoneConroller damageZone)
    {
        _fear = fear;
        _hunter = hunter;
        _damageZone = damageZone;
    }

    public override AIBehaviorModel Update()
    {
        if (_damageZone == null) return new AIBehaviorModel();
        var direction = (_damageZone.transform.position - _hunter.transform.position).normalized;
        if (_damageZone.IsDanger(_hunter.transform.position-direction*5))
        {
            return new AIBehaviorModel(_fear, new Vector2(direction.x, direction.z), false);
        }
        return new AIBehaviorModel();
    }
}
