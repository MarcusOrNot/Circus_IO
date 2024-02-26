#if UNITY_WEBGL || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexAds : IAds
{
    private YandexSDK _sdk;
    private Action<bool> _onShowInterstitial=null;
    private Action<bool> _onShowRewarded=null;
    private bool _rewardedGet = false;

    public YandexAds()
    {
        _sdk = YandexSDK.instance;
        _sdk.onRewardedAdReward = (string obj) => { _rewardedGet = true; };
        //_sdk.onRewardedAdOpened = (int place) => { _onShowRewarded.Invoke(true); };
        _sdk.onRewardedAdClosed = (int place) => { _onShowRewarded.Invoke(_rewardedGet); };
        _sdk.onRewardedAdError = (string obj) => { _onShowRewarded.Invoke(false); };

        _sdk.onInterstitialShown = () => { _onShowInterstitial.Invoke(true); };
        _sdk.onInterstitialFailed = (string obj) => { _onShowInterstitial.Invoke(false); };
    }

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
        _onShowInterstitial = onShowInterstitial;
        _sdk.ShowInterstitial();
        //throw new NotImplementedException();
        //onShowInterstitial?.Invoke(true);
        //_sdk.
        return true;
    }

    public bool ShowRewardedAd(Action<bool> onShowRewarded)
    {
        _rewardedGet = false;
        _onShowRewarded = onShowRewarded;
        _sdk.ShowRewarded("");
        //onShowRewarded?.Invoke(true);
        //throw new NotImplementedException();
        return true;

    }
}

#endif
