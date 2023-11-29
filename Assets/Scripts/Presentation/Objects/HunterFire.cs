using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class HunterFire : MonoBehaviour
{
    private Hunter _hunter;

    private ParticleSystem _particles;
    private Vector3 _startParticleShapeScale;
    private float _startEmissionRate;
    private Vector3 _startParticleObjectScale;
    private float _startParticleSize;

    [SerializeField] private float _minBurningTime = 2f;
    private IEnumerator _burningRoutine = null;

    private bool _particleSystemIsActivated = false;

    private void Awake()
    {
        _hunter = GetComponentInParent<Hunter>();
        _particles = GetComponent<ParticleSystem>();
        if (_hunter == null || _particles == null) Destroy(gameObject);

        _hunter?.SetOnBurning(() => SetParticlesActivationState());
        _hunter?.SetOnScaleChanged((scale) => ChangeScale());


        if (_particles != null)
        {
            _startParticleObjectScale = transform.lossyScale;
            _startParticleShapeScale = _particles.shape.scale;
            _startEmissionRate = _particles.emission.rateOverTime.constant;
            _startParticleSize = _particles.sizeOverLifetime.sizeMultiplier;
        }
                
    }

    


    private void SetParticlesActivationState()
    {
        if (_burningRoutine != null) StopCoroutine(_burningRoutine);   
        _burningRoutine = Burning();
        StartCoroutine(_burningRoutine);
    }

    private void ChangeScale()
    { 
        if (_particleSystemIsActivated) _particles?.Pause();
        float scaleMultiplier = 0.5f + transform.lossyScale.x / _startParticleObjectScale.x / 2f;
        var shape = _particles.shape; shape.scale = _startParticleShapeScale * scaleMultiplier;
        var emission = _particles.emission; emission.rateOverTime = _startEmissionRate * scaleMultiplier;        
        var particleSize = _particles.sizeOverLifetime; particleSize.sizeMultiplier = _startParticleSize * scaleMultiplier;
        if (_particleSystemIsActivated) _particles?.Play();
    }

    private IEnumerator Burning()
    {
        _particleSystemIsActivated = true;
        _particles?.Play();
        yield return new WaitForSeconds(_minBurningTime);
        _particleSystemIsActivated = false;
        _particles?.Stop();
    }


}
