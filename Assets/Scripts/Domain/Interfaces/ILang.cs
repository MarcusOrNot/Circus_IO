using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILang
{
    public LangType GetCurrentLang();
    public void ChangeLang(LangType newLang);
}
