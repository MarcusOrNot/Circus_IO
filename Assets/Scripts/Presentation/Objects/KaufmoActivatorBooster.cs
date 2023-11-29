using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KaufmoActivatorBooster : Booster
{ 

    public override BoosterType GetBoosterType() => BoosterType.KAUFMO_CONVERTER;

    public float KaufmoModeTime { get => _kaufoModeTime; }
    [SerializeField][Range(10, 100)] private float _kaufoModeTime = 20f;
}
