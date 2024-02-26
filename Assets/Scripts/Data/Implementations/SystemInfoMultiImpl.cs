using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfoMultiImpl : ISystemInfo
{
    private SystemPrefsModel _systemPrefs;
    public SystemInfoMultiImpl(SystemPrefsModel systemPrefs)
    {
        _systemPrefs = systemPrefs;
    }

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

        /*var platform = GetPlatformType();
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
        }*/

        //var platform = GetPlatformType();

#if UNITY_ANDROID
            return ControlType.TOUCH_SCREEN;
#elif UNITY_WEBGL
        string yandexPlatform = YandexSDK.instance.DeviceInfo;
            //if (yandexPlatform==null) return ControlType.TOUCH_SCREEN;
            switch(yandexPlatform)
            {
                case "desktop": return ControlType.KEYBOARD;
            }
            return ControlType.TOUCH_SCREEN;
#endif
        return ControlType.KEYBOARD;
    }

    public LangType GetSystemLang(LangType defaultLanguage)
    {
        /*var gameShop = GetSystemPrefs().ShopType;
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
            if (gameShop == GameShopType.YANDEX_GAMES)
            {
                string yaLang = YandexSDK.instance.YandexLanguage;
                switch (yaLang)
                {
                    case "\"ru\"": return LangType.RUSSIAN;
                }
            }
        }*/

        var gameShop = GetSystemPrefs().ShopType;
#if UNITY_ANDROID
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English: return LangType.ENGLISH;
            case SystemLanguage.Russian: return LangType.RUSSIAN;
        }
#elif UNITY_WEBGL
    if (gameShop == GameShopType.YANDEX_GAMES)
            {
                string yaLang = YandexSDK.instance.YandexLanguage;
                switch (yaLang)
                {
                    case "\"ru\"": return LangType.RUSSIAN;
                }
            }
#endif

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

    public SystemPrefsModel GetSystemPrefs()
    {
        return _systemPrefs;
    }
}
