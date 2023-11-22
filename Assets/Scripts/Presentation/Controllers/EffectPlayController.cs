using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class EffectPlayController : MonoBehaviour, IAudioEffect
{
    [Inject] private EffectPlayService _effectPlayService;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void PlayEffect(SoundEffectType effect)
    {
        _effectPlayService.PlayEffect(effect, _audioSource);
    }
}
