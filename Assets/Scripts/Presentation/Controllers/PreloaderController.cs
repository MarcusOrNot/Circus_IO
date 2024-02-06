using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Zenject;

public class PreloaderController : MonoBehaviour
{
    [Inject] private ILang _lang;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        var currentLang = Info.GetSystemLanguage(LangType.ENGLISH);
        _lang.ChangeLang(currentLang);
        Utils.OpenScene(SceneType.MAIN_MENU);


        for (int level = 1; level <= 5; level++)
        {
            float experienceNeeded = GameStatService.GetNeedExpByLevel(level);
            Debug.Log($"Для достижения {level} уровня необходимо {experienceNeeded} опыта.");
        }

        int currentLevel = GameStatService.GetLevel(500);
        Debug.Log($"При наличии 500 опыта ваш уровень будет {currentLevel}.");
    }
}
