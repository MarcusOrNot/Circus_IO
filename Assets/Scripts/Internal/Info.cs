using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Info
{
    public static LangType GetSystemLanguage(LangType defaultLanguage)
    {
        switch(Application.systemLanguage)
        {
            case SystemLanguage.English: return LangType.ENGLISH;
            case SystemLanguage.Russian: return LangType.RUSSIAN;
        }
        return defaultLanguage;
    }
}
