using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public static class Analytics
{
    //public const string LEVEL_SUCCESS = "level_success";
    private const string LEVEL_STARTED = "level_started";
    private const string LEVEL_FAILED = "level_failed";
    private const string AD_REWARDED_SHOWN = "ad_rewarded_showen";
    private const string AD_INTERSTITIAL_SHOWEN = "ad_interstitial_shown";
    private const string RATE_CHOSEN = "rate_chosen";
    private const string FEED_LEAVE = "feed_leaving";
    private const string HAT_BOUGHT = "heat_baught";
    //public const string RIGHT_CHOOSE = "right_choose";
    //public const string WRONG_CHOOSE = "wrong_choose";
    //private const string PARAMETER_RATE = "rate_value";

    /*public const string PARAMETER_PROGRESS = "progress";
    public static void LogLevelStart(int level)
    {
        //Debug.Log("LEvel statrt = "+FirebaseAnalytics.ParameterLevel+", "+FirebaseAnalytics.ParameterAdFormat);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, FirebaseAnalytics.ParameterLevel, level);
    }*/
    public static void LogLevelStarted()
    {
        //FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, FirebaseAnalytics.ParameterLevel, level);
        FirebaseAnalytics.LogEvent(LEVEL_STARTED);
    }
    public static void LogLevelFinished()
    {
        //FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, FirebaseAnalytics.ParameterLevel, level);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
    }
    public static void LogLevelFailed()
    {
        //FirebaseAnalytics.LogEvent(LEVEL_FAILED, FirebaseAnalytics.ParameterLevel, level);
        FirebaseAnalytics.LogEvent(LEVEL_FAILED);
    }
    public static void LogAdRewardedShowen()
    {
        FirebaseAnalytics.LogEvent(AD_REWARDED_SHOWN);
    }
    public static void LogAdInterstitialShowen()
    {
        FirebaseAnalytics.LogEvent(AD_INTERSTITIAL_SHOWEN);
    }
    public static void LogRateChosen(int rate)
    {
        FirebaseAnalytics.LogEvent(AD_INTERSTITIAL_SHOWEN, FirebaseAnalytics.ParameterValue, rate);
    }
    public static void LogFeedLeaving()
    {
        FirebaseAnalytics.LogEvent(FEED_LEAVE);
    }
    public static void LogHatBought()
    {
        FirebaseAnalytics.LogEvent(HAT_BOUGHT);
    }


    /*public static void LogGameFinished()
    {
        FirebaseAnalytics.LogEvent(GAME_FINISHED);
    }
    public static void LogHelpRewardedAd()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, FirebaseAnalytics.ParameterAdFormat, "Help Rewarded Ad");
    }
    public static void LogFullScreenAd()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, FirebaseAnalytics.ParameterAdFormat, "FullScreen Ad");
    }*/
    /*public static void LogRightDifChosen(int progress, int level)
    {
        //FirebaseAnalytics.LogEvent(RIGHT_CHOOSE, PARAMETER_PROGRESS, progress);
        FirebaseAnalytics.LogEvent(RIGHT_CHOOSE, new Parameter[] { new Parameter(PARAMETER_PROGRESS, progress), new Parameter(FirebaseAnalytics.ParameterLevel, level) });
    }
    public static void LogWrongDifChosen(int progress, int level)
    {
        //FirebaseAnalytics.LogEvent(WRONG_CHOOSE, PARAMETER_PROGRESS, progress);
        FirebaseAnalytics.LogEvent(WRONG_CHOOSE, new Parameter[] { new Parameter(PARAMETER_PROGRESS, progress), new Parameter(FirebaseAnalytics.ParameterLevel, level) });
    }*/
}
