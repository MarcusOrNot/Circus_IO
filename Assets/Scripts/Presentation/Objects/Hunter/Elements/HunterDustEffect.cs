
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class HunterDustEffect : MonoBehaviour
{   
    private ParticleSystem _particles;    
    private float _startEmissionRate;
    private Vector3 _startParticleObjectScale;
    private bool _particleSystemIsActivated = false;


    private void Awake()
    {        
        _particles = GetComponent<ParticleSystem>();        

        Hunter hunter = GetComponentInParent<Hunter>();
        hunter?.SetOnBoostingStateChanged((isBoosting) => SetParticlesActivationState(isBoosting));
        hunter?.SetOnScaleChanged((scale) => CorrectParticleSystemDependingOnHunterScale());
        hunter?.SetOnDestroying(() => PlaceParticlesOutOfHunterAndDestroy());    
            
        _startParticleObjectScale = transform.lossyScale; 
        _startEmissionRate = _particles.emission.rateOverTime.constant;        
    }

    private void Start()
    {
        _particles?.Stop();
    }

    private void SetParticlesActivationState(bool isEnabled)
    {
        _particleSystemIsActivated = isEnabled;
        if (isEnabled) _particles?.Play(); else _particles?.Stop();            
    }

    private void CorrectParticleSystemDependingOnHunterScale()
    {
        if (_particleSystemIsActivated) _particles?.Pause();
        float scaleMultiplier = transform.lossyScale.x / _startParticleObjectScale.x;        
        var emission = _particles.emission; 
        emission.rateOverTime = _startEmissionRate * scaleMultiplier;
        if (_particleSystemIsActivated) _particles?.Play();                       
    }

    private void PlaceParticlesOutOfHunterAndDestroy()
    {        
        _particles.gameObject.transform.SetParent(null, true);
        _particles?.Stop();
        Destroy(gameObject, 2f);
    }
}
