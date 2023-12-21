
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]

public class EntityDestroyEffect : MonoBehaviour
{   
    private ParticleSystem _particles;  
        

    private void Awake()
    {
        GetComponentInParent<Entity>()?.SetOnEating(() => PlaceParticlesOutOfHunterAndDestroy());
        _particles = GetComponent<ParticleSystem>(); 
    }

    private void Start()
    {
        _particles?.Stop();
    }


    private void PlaceParticlesOutOfHunterAndDestroy()
    {
        _particles.gameObject.transform.parent = null;
        _particles?.Play();
        Destroy(gameObject, 1.5f);        
    }    

}
