using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HealthBooster : Booster
{ 

    public override BoosterType GetBoosterType() => BoosterType.HEALTH_BOOSTER;

    public int HealCount { get => _healCount; }
    [SerializeField] [Range (0, 10000)] private int _healCount = 200;
    
}
