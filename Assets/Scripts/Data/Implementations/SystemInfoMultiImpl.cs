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
        var platform = GetPlatformType();
        if (platform == PlatformType.ANDROID)
        {
            return ControlType.TOUCH_SCREEN;
        }
        else if (platform == PlatformType.WEB_GL)
        {
            return ControlType.KEYBOARD;
        }
        return ControlType.KEYBOARD;
    }
}
