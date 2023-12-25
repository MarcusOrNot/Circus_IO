using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrakingEffectBooster : MonoBehaviour
{
    private bool _isTriggered = false;
    private GameObject _parent = null;
    private void OnCollisionEnter(Collision collision)
    {
        if (_isTriggered==false)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IBrakableMoving>()?.BrakeOn();
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<IBrakableMoving>()?.BrakeOff();
    }

    public void SetParentObject(GameObject parent)
    {
        _parent = parent;
    }
}
