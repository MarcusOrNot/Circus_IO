using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AdService
{
    private const int INTERSTITIAL_PAUSE_SECONDS=300;
    private IAds _ads;
    private IData _data;
    [Inject]
    public AdService(IAds ads, IData data)
    {
        _ads = ads;
        _data = data;
    }
    public bool IsAllowIntertitial()
    {
        var secondsElapsed = Utils.GetSecondsElapsed(_data.LastAdDate);
        return secondsElapsed>INTERSTITIAL_PAUSE_SECONDS;
    }
    public bool ShowInterstitialAd(Action<bool> onShowInterstitial)
    {
        return _ads.ShowInterstitialAd((isShown) =>
        {
            if (isShown) {
                Analytics.LogAdInterstitialShowen();
                _data.LastAdDate = DateTime.Now;
            }
            onShowInterstitial(isShown);
        });
    }
    public bool ShowInterstitialIfAllowed(Action<bool> onShowInterstitial)
    {
        if (IsAllowIntertitial()) return ShowInterstitialAd(onShowInterstitial);
        return false;
    }
    public bool ShowRewardedAd(Action<bool> onShowRewarded)
    {
        return _ads.ShowRewardedAd((isShown) =>
        {
            if (isShown)
            {
                Analytics.LogAdRewardedShowen();
                _data.LastAdDate = DateTime.Now;
            }
            onShowRewarded(isShown);
        });
    }
    public void ShowBanner()=>_ads.ShowBanner();
    public void HideBanner() => _ads.HideBanner();
}
