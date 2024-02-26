using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization.Scripts;

public class SimpleLocalizationImpl : ILang
{
    private Dictionary<LangType, string> _langs = new Dictionary<LangType, string>() {
        {LangType.ENGLISH, "English" },
        {LangType.RUSSIAN, "Russian" }
    };

    public void ChangeLang(LangType newLang)
    {
        var value = _langs[newLang];
        if (value != null)
        {
            LocalizationManager.Language = value;
        }
    }

    public LangType GetCurrentLang()
    {
        return GetLangByString(LocalizationManager.Language);
    }

    public string GetCurrentLangText(string key)
    {
        return LocalizationManager.Localize(key);
    }

    private LangType GetLangByString(string langString)
    {
        foreach (var item in _langs)
        {
            if (item.Value == langString)
                return item.Key;
        }
        return LangType.ENGLISH;
    }

    /*private string LangToString(LangType lang)
    {
        string res = "English";
        switch (lang)
        {
            case LangType.DEFAULT:
                break;
            case LangType.ENGLISH:
                res = "English";
                break;
            case LangType.RUSSIAN:
                res = "Russian";
                break;
        }
        return res;
    }

    private LangType StringToLang()
    {

    }*/
}
