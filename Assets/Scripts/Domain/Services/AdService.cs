using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AdService
{
    private const int INTERSTITIAL_PAUSE_SECONDS=180;
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
        var canShow = _ads.ShowInterstitialAd((isShown) =>
        {
            //_music.Continue();
            if (isShown) {
                Info.Analytics.LogAdInterstitialShowen();
                _data.LastAdDate = DateTime.Now;
            }
            onShowInterstitial(isShown);
        });
        //if (canShow == true) _music.Pause();
        return canShow;
    }
    public bool ShowInterstitialIfAllowed(Action<bool> onShowInterstitial)
    {
        if (IsAllowIntertitial()) return ShowInterstitialAd(onShowInterstitial);
        return false;
    }
    public bool ShowRewardedAd(Action<bool> onShowRewarded)
    {
        var canShow = _ads.ShowRewardedAd((isShown) =>
        {
            //_music.Continue();
            if (isShown)
            {
                Info.Analytics.LogAdRewardedShowen();
                //_data.LastAdDate = DateTime.Now;
            }
            onShowRewarded(isShown);
        });
        //if (canShow == true) _music.Pause();
        return canShow;
    }
    public void ShowBanner()=>_ads.ShowBanner();
    public void HideBanner() => _ads.HideBanner();
}
