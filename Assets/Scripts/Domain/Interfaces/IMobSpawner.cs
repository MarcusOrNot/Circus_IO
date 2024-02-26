using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMobSpawner
{
    public void SpawnAtLocation(HunterType hunterType, HatType hat, Vector3 location, int startLifes);
}
