using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Zenject;

public class PreloaderController : MonoBehaviour
{
    [Inject] private ILang _lang;
    [Inject] private ISystemInfo _systemInfo;
    [Inject] private StatDataService _statDataService;
    //[SerializeField] YandexSDK _yandexSDKPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        var currentLang = _systemInfo.GetSystemLang(LangType.ENGLISH); //Info.GetSystemLanguage(LangType.ENGLISH);
        //Debug.Log("Using lang is "+currentLang.ToString());
        _lang.ChangeLang(currentLang);
        //Utils.OpenScene(SceneType.MAIN_MENU);

        _statDataService.InitStartData((success) =>
        {
            Utils.OpenScene(SceneType.MAIN_MENU);
        });

        /*for (int level = 1; level <= 5; level++)
        {
            float experienceNeeded = GameStatService.GetNeedExpByLevel(level);
            Debug.Log($"Для достижения {level} уровня необходимо {experienceNeeded} опыта.");
        }

        int currentLevel = GameStatService.GetLevel(500);
        Debug.Log($"При наличии 500 опыта ваш уровень будет {currentLevel}.");*/
        /*var gameShop = _systemInfo.GetSystemPrefs().ShopType;
        switch (_systemInfo.GetPlatformType())
        {
            case PlatformType.ANDROID:
                break;
            case PlatformType.WEB_GL:
                if (gameShop==GameShopType.YANDEX_GAMES)
                {
                    //Загрузка облачных сохранений
                }
                //Instantiate(_yandexSDKPrefab);
                break;
            case PlatformType.PC:
                break;
        }*/
    }
}
