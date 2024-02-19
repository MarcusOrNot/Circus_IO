using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Zenject;

public class PreloaderController : MonoBehaviour
{
    [Inject] private ILang _lang;
    [Inject] private ISystemInfo _systemInfo;
    //[SerializeField] YandexSDK _yandexSDKPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        var currentLang = _systemInfo.GetSystemLang(LangType.ENGLISH); //Info.GetSystemLanguage(LangType.ENGLISH);
        //Debug.Log("Using lang is "+currentLang.ToString());
        _lang.ChangeLang(currentLang);
        Utils.OpenScene(SceneType.MAIN_MENU);


        /*for (int level = 1; level <= 5; level++)
        {
            float experienceNeeded = GameStatService.GetNeedExpByLevel(level);
            Debug.Log($"��� ���������� {level} ������ ���������� {experienceNeeded} �����.");
        }

        int currentLevel = GameStatService.GetLevel(500);
        Debug.Log($"��� ������� 500 ����� ��� ������� ����� {currentLevel}.");*/

        /*switch (_systemInfo.GetPlatformType())
        {
            case PlatformType.ANDROID:
                break;
            case PlatformType.WEB_GL:
                //Instantiate(_yandexSDKPrefab);
                break;
            case PlatformType.PC:
                break;
        }*/
    }
}
