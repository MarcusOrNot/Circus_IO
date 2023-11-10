using UnityEngine;


[System.Serializable]

public class HunterModel
{
    public int StartEntity = 10;
    public float BoostValue = 30f;

    public HunterColor HunterColor;
    public float DarkColorMultiplier = 0.25f;

    public float BoostTime = 0.5f;
    public float BoostRestartTime = 7f;
    public int BoostPrice = 0;

    public bool IsScaleDependFromHealth = true;

    public Renderer[] ColoredComponents;    
}
