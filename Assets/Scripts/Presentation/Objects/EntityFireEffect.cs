using AppodealStack.UnityEditor.SDKManager.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class EntityFireEffect : MonoBehaviour
{    
    private ParticleSystem _particles;  


    private void Awake()
    {        
        _particles = GetComponent<ParticleSystem>();        
        GetComponentInParent<Entity>()?.SetOnBurning(() => StartBurning());  
    }

    private void Start()
    {
        _particles?.Stop();
    }


    private void StartBurning()
    {
        _particles.gameObject.transform.parent = null;
        _particles?.Play();
        StartCoroutine(StopBurningAndDestroy()); 
    }

    private IEnumerator StopBurningAndDestroy() 
    {
        yield return new WaitForSeconds(1f);
        _particles?.Stop();
        Destroy(gameObject, 3f);
    }
    
}
