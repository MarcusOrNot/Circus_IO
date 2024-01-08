using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PanelLose : MonoBehaviour
{
    [Inject] private IAds _ads;
    [Inject] private LevelStatService _levelStat;
    public void ShowPanel()
    {
        gameObject.SetActive(true);
        _ads.ShowBanner();
    }

    public void HidePanel() 
    {
        _ads.HideBanner();
        gameObject.SetActive(false);
    }

    public void OpenMainMenu()
    {
        Utils.OpenScene(SceneType.MAIN_MENU);
    }
}
