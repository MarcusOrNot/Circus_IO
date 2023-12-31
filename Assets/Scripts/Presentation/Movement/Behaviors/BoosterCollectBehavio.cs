using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterCollectBehavior : AIBehavior
{
    private float _appetiteBooster;
    private Transform _transform;
    private Hunter _hunter;
    //private DamageZoneConroller _damageZone;
    public BoosterCollectBehavior(float appetiteBooster, Transform sourceTransform, Hunter hunter)
    {
        _appetiteBooster = appetiteBooster;
        _transform = sourceTransform;
        _hunter = hunter;
        //_damageZone = damageZone;
    }

    public override AIBehaviorModel Update()
    {
        //���� ����� ������
        Booster nearest = GetNearestObject<Booster>(_transform.position, null);
        
        if (nearest != null && _hunter.KaufmoIsActive == false)
        {
            var between = (nearest.transform.position - _transform.position).normalized;
            //Debug.Log("cross " + between.ToString());            
            //_direction = new Vector2(between.x, between.z);
            return new AIBehaviorModel(_appetiteBooster, new Vector2(between.x, between.z), false);
        }


        return new AIBehaviorModel();
    }
}
