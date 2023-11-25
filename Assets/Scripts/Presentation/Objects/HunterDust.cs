using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterDust : MonoBehaviour
{
    private Hunter _hunter;

    private ParticleSystem _dustParticles;
    private Vector3 _startDustParticleShapeScale;
    private float _startEmissionRate;

    private void Awake()
    {
        _hunter = GetComponentInParent<Hunter>();
        _dustParticles = GetComponentInChildren<ParticleSystem>();
        if (_hunter == null || _dustParticles == null) Destroy(gameObject);

        _hunter?.SetOnBoostingStateChanged(isBoosting => DustParticlesOn(isBoosting));
        _hunter?.SetOnScaleChanged(scale => ChangeScale(scale));
                
        if (_dustParticles != null)
        {
            _startDustParticleShapeScale = _dustParticles.shape.scale;
            _startEmissionRate = _dustParticles.emission.rateOverTime.constant;
        }
    }

    private void Start()
    {
        _dustParticles?.Stop();
    }

    private void DustParticlesOn(bool isEnabled)
    {
        if (isEnabled) _dustParticles?.Play();
        else _dustParticles?.Stop();
    }

    private void ChangeScale(Vector3 scale)
    {       
        var shape = _dustParticles.shape; shape.scale = Vector3.Scale(_startDustParticleShapeScale, scale);
        var emission = _dustParticles.emission; emission.rateOverTime = _startEmissionRate * scale.x; 
    }
    
}
