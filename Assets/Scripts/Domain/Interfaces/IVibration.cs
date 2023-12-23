using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVibration
{
    /*public const int DURATION_MICRO = 100;
    public const int DURATION_MINI = 100;
    public const int DURATION_MIDDLE = 250;
    public const int DURATION_HUGE = 500;*/

    void Play();
    void PlayMillis(int millis);
    void StopVibro();
}
