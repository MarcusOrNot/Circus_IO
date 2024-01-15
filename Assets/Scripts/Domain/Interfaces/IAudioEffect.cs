using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioEffect
{
    public void PlayEffect(SoundEffectType effect);
    public void PlayEffectConstantly(SoundEffectType effect);
}
