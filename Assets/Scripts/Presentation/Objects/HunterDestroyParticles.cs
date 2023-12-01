using AppodealStack.UnityEditor.SDKManager.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class HunterDestroyParticles : MonoBehaviour
{
    private Hunter _hunter;

    private ParticleSystem _particles;  

    

    private void Awake()
    {
        _hunter = GetComponentInParent<Hunter>();
        _particles = GetComponent<ParticleSystem>();
        if (_hunter == null || _particles == null) { Destroy(gameObject); }

        _hunter?.SetOnDestroying(() => ActivateParticles());        
                
    }

    private void Start()
    {
        var mainPreferences = _particles.main; mainPreferences.startColor = _hunter.Model.Color;
        _particles?.Stop();
    }


    private void ActivateParticles()
    {
        
        _particles.gameObject.transform.parent = null;
        _particles?.Play();
        Destroy(gameObject, 2f);        
    }

    

}
