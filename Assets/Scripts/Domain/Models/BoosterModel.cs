using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]

public class BoosterModel
{
    public BoosterType BoosterType;

    [Range (0, 10000)] public int HealCount = 200;
    [Range(10, 100)] public float KaufoModeTime = 20f;
}
