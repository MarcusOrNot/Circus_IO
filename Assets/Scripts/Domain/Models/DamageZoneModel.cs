using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageZoneModel
{
    public int TimeSeconds;
    public float Size;

    public DamageZoneModel(int timeSeconds, float size)
    {
        TimeSeconds = timeSeconds;
        Size = size;
    }
}
