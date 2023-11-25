using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettings
{
    public bool SoundOn { get; set; }
    public void SetOnSettingChanged(ISettingsObserver observer);
    public void RemoveOnSettingChanged(ISettingsObserver observer);
    public void NotifyObservers(SettingType setting);
}