using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class KaufmoActivatorBoosterModel
{  
    [Range(1, 100)] public float KaufoModeTime = 20f;
    [Range(0, 1)] public float FlickingTimeFraction = 0.15f;
    [Range (0.1f, 2f)] public float FlickingSpeed = 0.3f;

}
