using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PanelLose : MonoBehaviour
{
    [Inject] private IMusicPlayer _music;
    [Inject] private IAudioEffect _effect;
    [SerializeField] private TextMeshProUGUI _timeStatValue;
    [SerializeField] private TextMeshProUGUI _lifesStatValue;
    [SerializeField] private TextMeshProUGUI _diethsStatValue;
    [SerializeField] private TextMeshProUGUI _expStatValue;
    [SerializeField] private TextMeshProUGUI _coinsStatValue;


    [Inject] private IAds _ads;
    [Inject] private LevelStatService _levelStat;
    //[Inject] private GameStatService _gameStatService;
    [Inject] private StatDataService _gameStats;
    public void ShowPanel()
    {
        gameObject.SetActive(true);
        _ads.ShowBanner();

        _timeStatValue.text = _levelStat.SecondsElapsed.ToString();
        _lifesStatValue.text = _levelStat.MaxPlayerHealth.ToString();
        _diethsStatValue.text = _levelStat.HuntersEaten.ToString();
        //_expStatValue.text = _levelStat.GetExp().ToString();
        var resCoins = GameStatService.CalculateLoseCoins(_levelStat.SecondsElapsed, _levelStat.HuntersEaten, _levelStat.MaxPlayerHealth);
        var exp = GameStatService.GetExpFromCoins(resCoins);
        _coinsStatValue.text = resCoins.ToString();
        _expStatValue.text = exp.ToString();
        

        _gameStats.ChangeGameStat(GameStatsType.COINS, resCoins);
        _gameStats.ChangeGameStat(GameStatsType.EXP, exp);

        _music.Stop();
        _effect.PlayEffectConstantly(SoundEffectType.LEVEL_FAILED);
    }

    public void HidePanel() 
    {
        _ads.HideBanner();
        gameObject.SetActive(false);
    }

    public void OpenMainMenu()
    {
        Utils.OpenScene(SceneType.MAIN_MENU);
    }
}
