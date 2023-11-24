using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviorModel
{
    public float Weight = 0;
    public Vector2 Direction = Vector2.zero;
    public bool Accelerate = false;

    public AIBehaviorModel() { }

    public AIBehaviorModel(float weight, Vector2 direction, bool accelerate) {
        Weight = weight;
        Direction = direction;
        Accelerate = accelerate;
    }
}
