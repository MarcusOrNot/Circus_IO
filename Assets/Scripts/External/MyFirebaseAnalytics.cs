#if UNITY_ANDROID || UNITY_EDITOR
using Firebase.Analytics;

public class MyFirebaseAnalytics: IAnalytics
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
    public void LogLevelStarted()
    {
        //FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, FirebaseAnalytics.ParameterLevel, level);
        FirebaseAnalytics.LogEvent(LEVEL_STARTED);
    }
    public void LogLevelFinished()
    {
        //FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, FirebaseAnalytics.ParameterLevel, level);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
    }
    public void LogLevelFailed()
    {
        //FirebaseAnalytics.LogEvent(LEVEL_FAILED, FirebaseAnalytics.ParameterLevel, level);
        FirebaseAnalytics.LogEvent(LEVEL_FAILED);
    }
    public void LogAdRewardedShowen()
    {
        FirebaseAnalytics.LogEvent(AD_REWARDED_SHOWN);
    }
    public void LogAdInterstitialShowen()
    {
        FirebaseAnalytics.LogEvent(AD_INTERSTITIAL_SHOWEN);
    }
    public void LogRateChosen(int rate)
    {
        FirebaseAnalytics.LogEvent(AD_INTERSTITIAL_SHOWEN, FirebaseAnalytics.ParameterValue, rate);
    }
    public void LogFeedLeaving()
    {
        FirebaseAnalytics.LogEvent(FEED_LEAVE);
    }
    public void LogHatBought()
    {
        FirebaseAnalytics.LogEvent(HAT_BOUGHT);
    }
}

#endif