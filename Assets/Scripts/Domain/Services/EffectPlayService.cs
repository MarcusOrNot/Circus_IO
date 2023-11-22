using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EffectPlayService
{
    private List<SoundEffectModel> _effects;
    [Inject]
    public EffectPlayService(List<SoundEffectModel> effects)
    {
        _effects = effects;
    }
    public void PlayEffect(SoundEffectType effect, AudioSource source)
    {
        var effectSound = _effects.Find(e=>e.Effect==effect);
        if (effectSound != null)
        {
            source.clip = effectSound.EffectSource;
            source.volume = effectSound.Volume;
            source.Play();
        }
    }
}
