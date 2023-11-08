using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour, IGameUI
{
    [SerializeField] private TextMeshProUGUI _lifesValueText;
    public void SetLifesValue(int lifes)
    {
        _lifesValueText.text = lifes.ToString();
    }
}
