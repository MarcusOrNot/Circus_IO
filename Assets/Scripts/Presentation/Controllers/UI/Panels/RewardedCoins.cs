using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class RewardedCoins : MonoBehaviour
{
    [Inject] private AdService _ads;
    [Inject] private IGameStats _gameStats;
    [Inject] private IMusicPlayer _musicPlayer;
    public int RewardValue = 100;
    [SerializeField] private TextMeshProUGUI _rewardText;

    

    private void OnEnable()
    {
        _rewardText.text = RewardValue.ToString();
    }

    public void ShowRewarded()
    {
        var isReady = _ads.ShowRewardedAd((isShown) =>
        {
            if (isShown)
            {
                _gameStats.ChangeGameStat(GameStatsType.COINS, RewardValue);
            }
            Close();
        });
        if (isReady) _musicPlayer.Pause();
        else Close();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        _musicPlayer.Continue();
        gameObject.SetActive(false);
    }
}
