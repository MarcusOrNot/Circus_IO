using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KaufmoActivatorBooster : Booster
{
    [SerializeField] private KaufmoActivatorBoosterModel _model;

    public KaufmoActivatorBoosterModel Model { get => _model; }
    


    public override BoosterType GetBoosterType() => BoosterType.KAUFMO_CONVERTER;

    
}
