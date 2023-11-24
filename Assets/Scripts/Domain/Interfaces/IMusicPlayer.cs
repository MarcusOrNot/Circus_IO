using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMusicPlayer
{
    public void PlayRandom();
    public void PlayMusic(int musicPos);
    public void Stop();
    public void Pause();
    public void Continue();
}
