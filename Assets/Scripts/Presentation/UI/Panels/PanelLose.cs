using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PanelLose : MonoBehaviour
{
    [Inject] private IAds _ads;
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
}
