
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class BoosterFireParticles : MonoBehaviour
{ 
    private ParticleSystem _particles;  


    private void Awake()
    {        
        _particles = GetComponent<ParticleSystem>();        
        GetComponentInParent<Booster>()?.SetOnBurning(() => StartBurning());     
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
