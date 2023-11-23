using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class AIBehavior
{
    public abstract AIBehaviorModel Update();

    protected T GetNearestObject<T>(Vector3 position) where T : MonoBehaviour
    {
        return GetNearestObject<T>(position, null, null);
    }
    protected T GetNearestObject<T>(Vector3 position, T exceptObject, DamageZoneConroller damageZone) where T: MonoBehaviour
    {
        float currentDist = 1000000.0f;
        T nearest = null;

        //var allFood = GameObject.FindObjectsOfTypeAll(typeof(Entity)) as Entity[];
        var allObjects = GameObject.FindObjectsOfType<T>().ToList();
        allObjects.Remove(exceptObject);

        foreach (var elem in allObjects)
        {
            var dist = Vector3.Distance(elem.transform.position, position);
            if (dist < currentDist) //&& damageZone!=null?true:damageZone.IsDanger(elem.transform.position))
            {
                nearest = elem;
                currentDist = dist;
            }
        }
        return nearest;
    }
}
