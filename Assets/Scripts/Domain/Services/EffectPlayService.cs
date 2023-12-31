using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EffectPlayService
{
    private List<SoundEffectModel> _effects;
    private ISettings _settings;
    [Inject]
    public EffectPlayService(List<SoundEffectModel> effects, ISettings settings)
    {
        _effects = effects;
        _settings = settings;
    }
    public void PlayEffect(SoundEffectType effect, AudioSource source)
    {
        if (_settings.SoundOn==false) { return; }
        var effectSound = _effects.Find(e=>e.Effect==effect);
        if (effectSound != null)
        {
            source.clip = effectSound.EffectSource;
            source.volume = effectSound.Volume;
            source.Play();
        }
    }
}
