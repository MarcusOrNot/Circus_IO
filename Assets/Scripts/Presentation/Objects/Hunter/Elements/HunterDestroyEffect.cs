
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class HunterDestroyEffect : MonoBehaviour
{    
    private ParticleSystem _particles;
    private EffectPlayController _soundEffectsController;


    private void Awake()
    {        
        _particles = GetComponent<ParticleSystem>();  
        GetComponentInParent<Hunter>()?.SetOnDestroying(() => PlaceParticlesOutOfHunterAndDestroy());
        _soundEffectsController = GetComponent<EffectPlayController>();
    }

    private void Start()
    {        
        _particles?.Stop();
    }


    private void PlaceParticlesOutOfHunterAndDestroy()
    {        
        _particles.gameObject.transform.parent = null;
        _particles.Play();
        Destroy(gameObject, 2f);
        _soundEffectsController?.PlayEffect(SoundEffectType.HUNTER_DEATH);
    }   
}
