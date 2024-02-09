using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfoMultiImpl : ISystemInfo
{
    public PlatformType GetPlatformType()
    {
#if UNITY_ANDROID
        return PlatformType.ANDROID;
#elif UNITY_WEBGL
        return PlatformType.WEB_GL;
#else
        return PlatformType.PC;
#endif
    }

    public ControlType GetControlType()
    {
        if (IsEditor()) return ControlType.KEYBOARD;

        var platform = GetPlatformType();
        if (platform == PlatformType.ANDROID)
        {
            return ControlType.TOUCH_SCREEN;
        }
        else if (platform == PlatformType.WEB_GL)
        {
            string yandexPlatform = YandexSDK.instance.DeviceInfo;
            //if (yandexPlatform==null) return ControlType.TOUCH_SCREEN;
            switch(yandexPlatform)
            {
                case "desktop": return ControlType.KEYBOARD;
            }
            return ControlType.TOUCH_SCREEN;
        }
        return ControlType.KEYBOARD;
    }

    public LangType GetSystemLang(LangType defaultLanguage)
    {
        var platform = GetPlatformType();
        if (platform == PlatformType.ANDROID)
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English: return LangType.ENGLISH;
                case SystemLanguage.Russian: return LangType.RUSSIAN;
            }
        }
        else if (platform == PlatformType.WEB_GL) 
        {
            string yaLang = YandexSDK.instance.YandexLanguage;
            switch(yaLang)
            {
                case "ru": return LangType.RUSSIAN;
            }
        }
        return defaultLanguage;
    }

    private bool IsEditor()
    {
#if UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
