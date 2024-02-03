public interface IAnalytics
{
    public void LogLevelStarted();
    public void LogLevelFinished();
    public void LogLevelFailed();
    public void LogAdRewardedShowen();
    public void LogAdInterstitialShowen();
    public void LogRateChosen(int rate);
    public void LogFeedLeaving();
    public void LogHatBought();
}
