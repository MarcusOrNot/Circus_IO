using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static DateTime DateFromString(string base_string)
    {
        return DateTime.ParseExact(base_string, "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string DateToString(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
    }
    public static double GetSecondsElapsed(DateTime fromDate)
    {
        return (DateTime.Now - fromDate).TotalSeconds;
    }
    public static void OpenScene(SceneType scene)
    {
        int sceneNum = 0;
        switch (scene)
        {
            case SceneType.MAIN_MENU:
                sceneNum = 0;
                break;
            case SceneType.GAME_ROYAL_BATTLE:
                sceneNum = 1;
                break;
            case SceneType.LOADER_SCENE:
                sceneNum = 2;
                break;
        }
        SceneManager.LoadScene(sceneNum);
    }
    public static Vector3 GetRandomPlace(Vector3 centerPosition, float areaSize)
    {
        float middle = areaSize / 2;
        return new Vector3(middle - UnityEngine.Random.Range(0, areaSize), centerPosition.y, middle - UnityEngine.Random.Range(0, areaSize));
        //return new Vector3(middle - _rnd.Next(GenerationAreaSize), transform.position.y, middle - _rnd.Next(GenerationAreaSize));
    }
}
