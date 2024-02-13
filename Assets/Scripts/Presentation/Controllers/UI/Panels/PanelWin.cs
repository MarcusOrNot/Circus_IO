using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PanelWin : MonoBehaviour
{
    [Inject] private IAds _ads;
    [Inject] private IGameStats _gameStats;
    [Inject] private LevelStatService _levelStatService;
    //[Inject] private GameStatService _statService;
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
        var huntersEaten = _levelStatService.HuntersEaten;
        var charLifes = Level.Instance.GetPlayer()?.GetLifes()==null?0:_levelStatService.MaxPlayerHealth;
        var coins = GameStatService.CalculateWinnerCoins(charLifes, huntersEaten);
        var exp = GameStatService.GetExpFromCoins(coins);
        _gameStats.ChangeGameStat(GameStatsType.COINS, coins);
        _gameStats.ChangeGameStat(GameStatsType.EXP, exp);
        _gameStats.ChangeGameStat(GameStatsType.KOEF_DIFFICULTY, 1);
        Debug.Log("Current LEvel exp "+_gameStats.GetStat(GameStatsType.EXP).ToString());

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
