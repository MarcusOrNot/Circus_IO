using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLocalPrefs : ISettings
{
    private List<ISettingsObserver> _observers = new List<ISettingsObserver>();
    private const string SOUND_SETTING = "Settings_sound";
    private const string HAT_SETTING = "Settings_hat";
    public bool SoundOn { get => PlayerPrefs.GetInt(SOUND_SETTING, 1) == 1; set => PlayerPrefs.SetInt(SOUND_SETTING, value?1:0); }
    public HatType ChosenHat { get => (HatType) PlayerPrefs.GetInt(HAT_SETTING, 0); set => PlayerPrefs.SetInt(HAT_SETTING, (int) value); }

    public void NotifyObservers(SettingType setting)
    {
        foreach (var observer in _observers)
            observer.Notify(setting);
    }

    public void RemoveOnSettingChanged(ISettingsObserver observer)
    {
        _observers.Remove(observer);
    }

    public void SetOnSettingChanged(ISettingsObserver observer)
    {
        _observers.Add(observer);
    }
}
