using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour, IGameUI
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TextMeshProUGUI _lifesValueText;
    public void SetLifesValue(int lifes)
    {
        _lifesValueText.text = lifes.ToString();
    }

    public void ShowGameOver()
    {
        HideAll();
        _gameOverPanel.SetActive(true);
    }

    public void ShowWin()
    {
        HideAll();
        _winPanel.SetActive(true);
    }

    private void HideAll()
    {
        _gameOverPanel.SetActive(false);
        _winPanel.SetActive(false);
    }

    public void RestartLevel()
    {
        Level.Instance.RestartLevel();
    }
}
