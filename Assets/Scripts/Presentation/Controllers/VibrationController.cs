using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationController : MonoBehaviour, IVibration
{
    private void Awake()
    {
        Vibration.Init();
    }
    public void Play()
    {
        //Handheld.Vibrate();
        PlayMillis(20);
    }

    public void PlayMillis(int millis)
    {
        //StopVibro();
        //StartCoroutine(VibroCoroutine(millis));
#if UNITY_ANDROID
    Vibration.VibrateAndroid(millis);
#elif UNITY_WEBGL

#endif
    }

    public void StopVibro()
    {
        //StopAllCoroutines();
        Vibration.CancelAndroid();
    }

    /*private IEnumerator VibroCoroutine(int millis)
    {
        if (millis>0)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(millis/1000f);
            //Handheld.Vibrate();
            //Handheld.StopVibration();
        }
    }*/
}
