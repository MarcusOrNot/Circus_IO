using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppodealAds : MonoBehaviour, IAds
{
    private Action<bool> _onShowInterstitial;
    private Action<bool> _onShowRewarded;
    private bool _bannerShown = false;

    // Start is called before the first frame update
    void Start()
    {
        int adTypes = AppodealAdType.Interstitial | AppodealAdType.Banner | AppodealAdType.RewardedVideo;
        string appKey = "47a27d5764228d42434b249e7a3f9507d1ae54fc1f29855b";
        //AppodealCallbacks.Sdk.OnInitialized += OnInitializationFinished;
        //Interstitial Callbacks
        AppodealCallbacks.Interstitial.OnShowFailed += delegate { _onShowInterstitial?.Invoke(false); };
        AppodealCallbacks.Interstitial.OnClosed += delegate { _onShowInterstitial?.Invoke(true); };
        AppodealCallbacks.RewardedVideo.OnShowFailed += delegate { _onShowRewarded?.Invoke(false); };
        AppodealCallbacks.RewardedVideo.OnClosed += RewardedVideo_OnClosed;
        AppodealCallbacks.Banner.OnShown += delegate { _bannerShown = true; };
        //Rewarded Callbacks
        
        Appodeal.Initialize(appKey, adTypes);
    }

    private void RewardedVideo_OnClosed(object sender, RewardedVideoClosedEventArgs e)
    {
        _onShowRewarded?.Invoke(e.Finished);
    }

    public void HideBanner()
    {
        //if (Appodeal.IsInitialized())
        if (_bannerShown)
        {
            _bannerShown = false;
            Appodeal.Hide(AppodealAdType.Banner);
        }
        //Appodeal.HideBannerView();
    }

    public void ShowBanner()
    {
        //if (Appodeal.IsLoaded(AppodealAdType.Banner))
            Appodeal.Show(AppodealShowStyle.BannerBottom);
    }

    public bool ShowInterstitialAd(Action<bool> onShowInterstitial)
    {
        if (Appodeal.IsLoaded(AppodealAdType.Interstitial))
        {
            _onShowInterstitial = onShowInterstitial;
            Appodeal.Show(AppodealShowStyle.Interstitial);
            return true;
        }
        return false;
    }

    public bool ShowRewardedAd(Action<bool> onShowRewarded)
    {
        if (Appodeal.IsLoaded(AppodealAdType.RewardedVideo))
        {
            _onShowRewarded = onShowRewarded;
            Appodeal.Show(AppodealShowStyle.RewardedVideo);
            return true;
        }
        return false;
    }
}
