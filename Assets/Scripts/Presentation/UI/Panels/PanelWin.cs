using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PanelWin : MonoBehaviour
{
    [Inject] private IAds _ads;
    [SerializeField] private TextMeshProUGUI _scoreValueText;
    public void Show()
    {
        gameObject.SetActive(true);
        if (Level.Instance.GetPlayer()!=null)
        {
            _scoreValueText.text = Level.Instance.GetPlayer().GetLifes().ToString();
        }
        _ads.ShowBanner();
    }

    public void Hide()
    {
        _ads.HideBanner();
        gameObject.SetActive(false);
    }
}
