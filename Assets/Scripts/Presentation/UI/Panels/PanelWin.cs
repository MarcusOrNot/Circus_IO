using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PanelWin : MonoBehaviour
{
    [Inject] private IAds _ads;
    [Inject] private IGameStats _gameStats;
    [SerializeField] private TextMeshProUGUI _scoreValueText;
    public void Show()
    {
        gameObject.SetActive(true);
        if (Level.Instance.GetPlayer()!=null)
        {
            int score = Level.Instance.GetPlayer().GetLifes() / 10;
            _scoreValueText.text = score.ToString();
            _gameStats.SetGameStat(GameStatsType.SCORE, _gameStats.GetStat(GameStatsType.SCORE)+score);
            
        }
        _ads.ShowBanner();
    }

    public void Hide()
    {
        _ads.HideBanner();
        gameObject.SetActive(false);
    }
}
