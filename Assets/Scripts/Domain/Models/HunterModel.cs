using UnityEngine;


[System.Serializable]

public class HunterModel
{
    [Range(1, 10000)] public int StartHealth = 10;

    [Range(2f, 5f)] public float BoostSpeedMultiplier = 3f;
    [Range(0.1f, 1f)] public float BoostTime = 0.5f;
    [Range(1.1f, 10f)] public float BoostRestartTime = 3f;
    [Range(0, 1000)] public int BoostPrice = 0;

    [Range(1.1f, 3.5f)] public float KaufmoSpeedMultiplier = 1.5f;
    
    public HunterType HunterType;    

    public bool IsScaleDependFromHealth = true;    
}
