using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class HunterSpawn : MonoBehaviour
{
    [SerializeField] protected HunterType _hunter;
    [Inject] protected HunterFactory _factory;
    public virtual void Start()
    {
        SetPosition(_factory.Spawn(_hunter));
        Destroy(gameObject);
    }
    /*protected Hunter Spawn()
    {
        var hunter = _factory.Spawn(_hunter);
        hunter.GetComponent<Rigidbody>().isKinematic = true;
        hunter.gameObject.transform.position = transform.position;
        hunter.GetComponent<Rigidbody>().isKinematic = false;
        return hunter;
    }*/
    protected void SetPosition(Hunter hunter)
    {
        hunter.GetComponent<Rigidbody>().isKinematic = true;
        hunter.gameObject.transform.position = transform.position;
        hunter.GetComponent<Rigidbody>().isKinematic = false;
    }
}
