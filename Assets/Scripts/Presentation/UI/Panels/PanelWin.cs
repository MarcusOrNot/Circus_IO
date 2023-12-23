using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PanelWin : MonoBehaviour
{
    private const int COMPLETE_SCORE = 35;
    [Inject] private IAds _ads;
    [Inject] private IGameStats _gameStats;
    [SerializeField] private TextMeshProUGUI _scoreValueText;
    [SerializeField] private TextMeshProUGUI _scoreComplete;
    [SerializeField] private TextMeshProUGUI _scoreHealth;
    public void Show()
    {
        _scoreComplete.text = "+"+ COMPLETE_SCORE.ToString();
        gameObject.SetActive(true);
        if (Level.Instance.GetPlayer()!=null)
        {
            int lifesBonus = Level.Instance.GetPlayer().GetLifes() / 10;
            int score = lifesBonus + COMPLETE_SCORE;
            _scoreHealth.text = "+"+ lifesBonus.ToString();
            _scoreValueText.text = score.ToString();
            _gameStats.SetGameStat(GameStatsType.COINS, _gameStats.GetStat(GameStatsType.COINS)+score);
            
        }
        _ads.ShowBanner();
    }

    public void Hide()
    {
        _ads.HideBanner();
        gameObject.SetActive(false);
    }
}
