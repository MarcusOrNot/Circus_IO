using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsObserver
{
    public void Notify(SettingType settingType);
}
