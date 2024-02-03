using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YandexAds : IAds
{
    public void HideBanner()
    {
        //throw new NotImplementedException();
    }

    public void ShowBanner()
    {
        //throw new NotImplementedException();
    }

    public bool ShowInterstitialAd(Action<bool> onShowInterstitial)
    {
        //throw new NotImplementedException();
        onShowInterstitial?.Invoke(true);
        return true;
    }

    public bool ShowRewardedAd(Action<bool> onShowRewarded)
    {
        onShowRewarded?.Invoke(true);
        //throw new NotImplementedException();
        return true;
    }
}
