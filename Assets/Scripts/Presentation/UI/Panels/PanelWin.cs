using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelWin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreValueText;
    public void Show()
    {
        gameObject.SetActive(true);
        if (Level.Instance.GetPlayer()!=null)
        {
            _scoreValueText.text = Level.Instance.GetPlayer().GetLifes().ToString();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
