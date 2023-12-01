using AppodealStack.UnityEditor.SDKManager.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class BoosterFireParticles : MonoBehaviour
{ 

    private Booster _booster;

    private ParticleSystem _particles;  

    

    private void Awake()
    {
        _booster = GetComponentInParent<Booster>();
        _particles = GetComponent<ParticleSystem>();
        if (_booster == null || _particles == null) Destroy(gameObject);

        _booster?.SetOnBurning(() => ActivateParticles());        
                
    }

    private void Start()
    {
        _particles?.Stop();
    }


    private void ActivateParticles()
    {
        _particles.gameObject.transform.parent = null;
        _particles?.Play();
        Destroy(gameObject, 3f);
        StartCoroutine(StopBurning());
    }

    private IEnumerator StopBurning() 
    {
        yield return new WaitForSeconds(1f);
        _particles?.Stop();
    }

}
