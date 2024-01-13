using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

public class PanelWin : MonoBehaviour
{
    private const int COMPLETE_SCORE = 35;
    [Inject] private IAds _ads;
    [Inject] private IGameStats _gameStats;
    [Inject] private GameStatService _statService;
    [SerializeField] private TextMeshProUGUI _coinsValueText;
    [SerializeField] private TextMeshProUGUI _expValueText;
    //[SerializeField] private TextMeshProUGUI _scoreComplete;
    //[SerializeField] private TextMeshProUGUI _scoreHealth;
    public void Show()
    {
        /*_scoreComplete.text = "+"+ COMPLETE_SCORE.ToString();
        gameObject.SetActive(true);
        if (Level.Instance.GetPlayer()!=null)
        {
            int lifesBonus = Level.Instance.GetPlayer().GetLifes() / 10;
            int score = lifesBonus + COMPLETE_SCORE;
            _scoreHealth.text = "+"+ lifesBonus.ToString();
            _scoreValueText.text = score.ToString();
            _gameStats.SetGameStat(GameStatsType.COINS, _gameStats.GetStat(GameStatsType.COINS)+score);
            
        }*/
        gameObject.SetActive(true);
        var charLifes = Level.Instance.GetPlayer()?.GetLifes()==null?0:1;
        var coins = GameStatService.CalculateWinnerCoins(charLifes, 2);
        var exp = coins * 7;
        _gameStats.SetGameStat(GameStatsType.COINS, _gameStats.GetStat(GameStatsType.COINS) + coins);
        _gameStats.SetGameStat(GameStatsType.EXP, _gameStats.GetStat(GameStatsType.EXP) + exp);

        _coinsValueText.text = coins.ToString();
        _expValueText.text = exp.ToString();
        //_coinsValueText.text = _statService.GetC
        _ads.ShowBanner();
    }

    public void Hide()
    {
        _ads.HideBanner();
        gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        Utils.OpenScene(SceneType.MAIN_MENU);
    }
}
