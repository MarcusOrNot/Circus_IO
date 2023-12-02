using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class HunterDust : MonoBehaviour
{
    private Hunter _hunter;

    private ParticleSystem _particles;
    private Vector3 _startParticleShapeScale;
    private float _startEmissionRate;
    private Vector3 _startParticleObjectScale;

    private bool _particleSystemIsActivated = false;

    private void Awake()
    {
        _hunter = GetComponentInParent<Hunter>();
        _particles = GetComponent<ParticleSystem>();
        if (_hunter == null || _particles == null) Destroy(gameObject);

        _hunter?.SetOnBoostingStateChanged((isBoosting) => SetParticlesActivationState(isBoosting));
        _hunter?.SetOnScaleChanged((scale) => ChangeScale());
        _hunter?.SetOnDestroying(() => SaveParticlesAfterHunterDestroying());
        
                
        if (_particles != null)
        {
            _startParticleObjectScale = transform.lossyScale;
            //_startParticleShapeScale = _particles.shape.scale;
            _startEmissionRate = _particles.emission.rateOverTime.constant;
        }
    }

    private void Start()
    {
        _particles?.Stop();
    }




    private void SetParticlesActivationState(bool isEnabled)
    {
        if (isEnabled)
        {
            _particleSystemIsActivated = true;
            _particles?.Play();
        }        
        else
        {
            _particleSystemIsActivated = false;
            _particles?.Stop();
        }
            
    }

    private void ChangeScale()
    {
        if (_particleSystemIsActivated) _particles?.Pause();
        float scaleMultiplier = transform.lossyScale.x / _startParticleObjectScale.x;
        //var shape = _particles.shape; shape.scale = _startParticleShapeScale * scaleMultiplier;
        var emission = _particles.emission; emission.rateOverTime = _startEmissionRate * scaleMultiplier;
        if (_particleSystemIsActivated) _particles?.Play();

                       
    }

    private void SaveParticlesAfterHunterDestroying()
    {
        //_particles.gameObject.transform.parent = null;
        _particles.gameObject.transform.SetParent(null, true);
        _particles?.Stop();
        Destroy(gameObject, 2f);
    }
}
