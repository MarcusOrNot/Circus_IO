using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PreloaderController : MonoBehaviour
{
    [Inject] private ILang _lang;
    // Start is called before the first frame update
    void Start()
    {
        var currentLang = Info.GetSystemLanguage(LangType.ENGLISH);
        _lang.ChangeLang(currentLang);
        Utils.OpenScene(SceneType.MAIN_MENU);
    }
}
