using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PanelWin : MonoBehaviour
{
    [Inject] private AdService _ads;
    [Inject] private StatDataService _gameStats;
    [Inject] private LevelStatService _levelStatService;
    [Inject] private IMusicPlayer _music;
    [Inject] private IAudioEffect _effect;
    [SerializeField] private TextMeshProUGUI _coinsValueText;
    [SerializeField] private TextMeshProUGUI _expValueText;
    private int _winCoins = 0;
    public void Show()
    {
        gameObject.SetActive(true);
        _music.Stop();

        /*if (_ads.ShowInterstitialIfAllowed((successfull) =>
        {
            ProcessWin();
        }) == false)
        {
            ProcessWin();
        }*/
        ProcessWin();


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

    private void ProcessWin()
    {
        var huntersEaten = _levelStatService.HuntersEaten;
        var charLifes = Level.Instance.GetPlayer()?.GetLifes() == null ? 0 : _levelStatService.MaxPlayerHealth;
        _winCoins = GameStatService.CalculateWinnerCoins(charLifes, huntersEaten);
        var exp = GameStatService.GetExpFromCoins(_winCoins);
        _gameStats.ChangeGameStats(new Dictionary<GameStatsType, int>()
        {
            {GameStatsType.COINS, _winCoins },
            {GameStatsType.EXP, exp },
            {GameStatsType.KOEF_DIFFICULTY, 1 }
        });
        /*_gameStats.ChangeGameStat(GameStatsType.COINS, _winCoins);
        _gameStats.ChangeGameStat(GameStatsType.EXP, exp);
        _gameStats.ChangeGameStat(GameStatsType.KOEF_DIFFICULTY, 1);*/

        _coinsValueText.text = _winCoins.ToString();
        _expValueText.text = exp.ToString();

        _effect.PlayEffectConstantly(SoundEffectType.LEVEL_COMPLETED);
    }

    public void ShowRewardedX2()
    {
        _ads.ShowRewardedAd((isResult) =>
        {
            if (isResult)
            {
                _effect.PlayEffectConstantly(SoundEffectType.LEVEL_COMPLETED);
                _gameStats.ChangeGameStat(GameStatsType.COINS, _winCoins);
                _coinsValueText.text = (_winCoins*2).ToString();
            }
        });
    }
}
