using System.Collections;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class HunterFireEffect : MonoBehaviour
{    
    private ParticleSystem _particles;    

    [SerializeField] private float _minBurningTime = 2f;
    private IEnumerator _burningRoutine = null;
    

    private void Awake()
    {        
        _particles = GetComponentInChildren<ParticleSystem>();

        Hunter hunter = GetComponentInParent<Hunter>();
        hunter?.SetOnBurning(() => StartBurning());        
        hunter?.SetOnDestroying(() => PlaceParticlesOutOfHunterAndDestroy());   
    }

    private void Start()
    {
        _particles?.Stop();
    }


    private void StartBurning()
    {
        if (_burningRoutine != null) StopCoroutine(_burningRoutine);   
        _burningRoutine = Burning();
        StartCoroutine(_burningRoutine);
    }
   
    private IEnumerator Burning()
    {        
        _particles?.Play();
        yield return new WaitForSeconds(_minBurningTime);        
        _particles?.Stop();
    }

    private void PlaceParticlesOutOfHunterAndDestroy()
    {
        _particles.gameObject.transform.parent = null;        
        _particles?.Stop();
        Destroy(gameObject, 2f);
    }
}
