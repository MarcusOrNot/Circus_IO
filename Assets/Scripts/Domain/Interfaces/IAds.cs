using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAds
{
    public bool ShowInterstitialAd(Action<bool> onShowInterstitial);
    public bool ShowRewardedAd(Action<bool> onShowRewarded);
    public void ShowBanner();
    public void HideBanner();
}
