using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class MobModel : MonoBehaviour
{
    public HunterType HunterType;
    public HatType HatType;
    public int StartLifes;

    public MobModel(HunterType hunterType, HatType hatType, int startLifes)
    {
        HunterType = hunterType;
        HatType = hatType;
        StartLifes = startLifes;
    }
}
