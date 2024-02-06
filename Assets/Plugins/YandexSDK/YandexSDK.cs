using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexSDK : MonoBehaviour {
    public static YandexSDK instance;
    [DllImport("__Internal")]
    private static extern void GetUserData();
    [DllImport("__Internal")]
    private static extern void ShowFullscreenAd();
    /// <summary>
    /// Returns an int value which is sent to index.html
    /// </summary>
    /// <param name="placement"></param>
    /// <returns></returns>
    [DllImport("__Internal")]
    private static extern int ShowRewardedAd(string placement);
    //[DllImport("__Internal")]
    //private static extern void GetReward();
    [DllImport("__Internal")]
    private static extern void AuthenticateUser();
    /*[DllImport("__Internal")]
    private static extern void InitPurchases();*/
    [DllImport("__Internal")]
    private static extern void Purchase(string id);
    [DllImport("__Internal")]
    private static extern void GetCatalog();
    [DllImport("__Internal")]
    private static extern void GetPurchases();
    [DllImport("__Internal")]
    private static extern void SetGameData(string gamedataString);
    [DllImport("__Internal")]
    private static extern void GetYandexGameData(string keyJSON);
    [DllImport("__Internal")]
    private static extern void SetGameStat(string statdataString);
    [DllImport("__Internal")]
    private static extern void GetYandexGameStat(string keyJSON);
    [DllImport("__Internal")]
    private static extern void GetYandexLeaderboard(string leaderboardName, int quantityTop, bool includeUser, int quantityAround);
    [DllImport("__Internal")]
    private static extern void SetYandexLeaderboardScore(string leaderboardName, int score);
    [DllImport("__Internal")]
    private static extern void GetLanguage();

    public UserData user;
    public string YandexLanguage { get; private set; }

    public event Action onUserDataReceived;

    public event Action<string> onGameDataReceived;
    public event Action<string> onGameStatReceived;

    public event Action<LeaderboardYandex> onLeaderboardReceived;
    public event Action<InapProduct[]> onGetCatalog;
    public event Action<PurchaseItem[]> onGetPurchases;

    public Action onInterstitialShown;
    public Action<string> onInterstitialFailed;
    /// <summary>
    /// Пользователь открыл рекламу
    /// </summary>
    public Action<int> onRewardedAdOpened;
    /// <summary>
    /// Пользователь должен получить награду за просмотр рекламы
    /// </summary>
    public Action<string> onRewardedAdReward;
    /// <summary>
    /// Пользователь закрыл рекламу
    /// </summary>
    public Action<int> onRewardedAdClosed;
    /// <summary>
    /// Вызов/просмотр рекламы повлёк за собой ошибку
    /// </summary>
    public Action<string> onRewardedAdError;
    /// <summary>
    /// Покупка успешно совершена
    /// </summary>
    public event Action<string> onPurchaseSuccess;
    /// <summary>
    /// Покупка не удалась: в консоли разработчика не добавлен товар с таким id,
    /// пользователь не авторизовался, передумал и закрыл окно оплаты,
    /// истекло отведенное на покупку время, не хватило денег и т. д.
    /// </summary>
    public event Action<string> onPurchaseFailed;
    /// Ошибка авторизации
    /// </summary>
    public event Action<string> onAuthFailed;

    public event Action onClose;

    public Queue<int> rewardedAdPlacementsAsInt = new Queue<int>();
    public Queue<string> rewardedAdsPlacements = new Queue<string>();

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Call this to ask user to authenticate
    /// </summary>
    public void Authenticate() {
        AuthenticateUser();
    }

    /// <summary>
    /// Call this to show interstitial ad. Don't call frequently. There is a 3 minute delay after each show.
    /// </summary>
    public void ShowInterstitial() {
        ShowFullscreenAd();
    }

    /// <summary>
    /// Call this to show rewarded ad
    /// </summary>
    /// <param name="placement"></param>
    public void ShowRewarded(string placement) {
        rewardedAdPlacementsAsInt.Enqueue(ShowRewardedAd(placement));
        rewardedAdsPlacements.Enqueue(placement);
    }
    
    /// <summary>
    /// Call this to receive user data
    /// </summary>
    public void RequestUserData() {
        GetUserData();
    }
    
    /*public void InitializePurchases() {
        InitPurchases();
    }*/

    public void ProcessPurchase(string id) {
        Purchase(id);
    }
    
    public void StoreUserData(string data) {
        user = JsonUtility.FromJson<UserData>(data);
        onUserDataReceived?.Invoke();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void OnInterstitialShown() {
        onInterstitialShown?.Invoke();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="error"></param>
    public void OnInterstitialError(string error) {
        onInterstitialFailed?.Invoke(error);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void OnRewardedOpen(int placement) {
        onRewardedAdOpened?.Invoke(placement);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void OnRewarded(int placement) {
        if (placement == rewardedAdPlacementsAsInt.Dequeue()) {
            onRewardedAdReward?.Invoke(rewardedAdsPlacements.Dequeue());
            //Debug.Log($"OnRewarded placement = {placement} onRewardedAdReward = {onRewardedAdReward == null}");
        }
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void OnRewardedClose(int placement) {
        onRewardedAdClosed?.Invoke(placement);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void OnRewardedError(string placement) {
        onRewardedAdError?.Invoke(placement);
        rewardedAdsPlacements.Clear();
        rewardedAdPlacementsAsInt.Clear();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="id"></param>
    public void OnPurchaseSuccessYandex(string id) {
        //Debug.Log("Purchase success with id  "+id);
        //onPurchaseSuccess?.Invoke(int.Parse(id));
        onPurchaseSuccess?.Invoke(id);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="error"></param>
    public void OnPurchaseFailed(string error) {
        onPurchaseFailed?.Invoke(error);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="error"></param>
    public void OnAuthFailed(string error)
    {
        onAuthFailed?.Invoke(error);
    }

    /// <summary>
    /// Browser tab has been closed
    /// </summary>
    /// <param name="error"></param>
    public void OnClose() {
        onClose?.Invoke();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void SetGameDataJSON(string jsonString)
    {
        SetGameData(jsonString);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void GameJSONReceived(string jsonString)
    {
        onGameDataReceived?.Invoke(jsonString);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void GameStatJSONReceived(string jsonString)
    {
        onGameStatReceived?.Invoke(jsonString);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void LeaderboardJSONReceived(string jsonString)
    {
        //Debug.Log("Leader board data is "+jsonString);
        onLeaderboardReceived?.Invoke(JsonUtility.FromJson<LeaderboardYandex>(jsonString));
    }

    private struct CatalogStruct
    {
        public InapProduct[] catalog;
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void InapCatalogJSONReceived(string jsonString)
    {
        //Debug.Log("Inap catalog is " + jsonString);
        CatalogStruct catalogStruct = JsonUtility.FromJson<CatalogStruct>(jsonString);
        onGetCatalog?.Invoke(catalogStruct.catalog);
    }

    private struct PurchaseStruct
    {
        public PurchaseItem[] purchases;
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void PurchasesJSONReceived(string jsonString)
    {
        //Debug.Log("Purchase is " + jsonString);
        onGetPurchases?.Invoke(JsonUtility.FromJson<PurchaseStruct>(jsonString).purchases);
    }

    public void SetStat(string jsonString)
    {
        SetGameStat(jsonString);
    }

    public void RequestGameData(string[] keys)
    {
        if (keys == null) GetYandexGameData("");
        else
        {
            RequestStruct req; req.keys = keys;
            GetYandexGameData(JsonUtility.ToJson(req));
        }
    }

    public void RequestLeaderboard(string leaderboardName, int quantityTop, bool includeUser, int quantityAround)
    {
        GetYandexLeaderboard(leaderboardName, quantityTop, includeUser, quantityAround);
    }

    public void RequestInapCatalog()
    {
        GetCatalog();
    }

    public void RequestPurchases()
    {
        GetPurchases();
    }

    public void SetLeaderboardScore(string leaderboardName, int score)
    {
        SetYandexLeaderboardScore(leaderboardName, score);
    }

    public void RequestStatData(string[] keys)
    {
        if (keys == null) GetYandexGameStat("");
        else
        {
            RequestStruct req; req.keys = keys;
            GetYandexGameStat(JsonUtility.ToJson(req));
        }
    }

    private struct RequestStruct
    {
        public string[] keys;
    }

    public void SetYandexLanguage(string json)
    {
        //Debug.Log("GetYandexLanguage = " + json);
        YandexLanguage = json;
    }

    public void RequestLanguage()
    {
        GetLanguage();
    }
}

public struct UserData {
    public string id;
    public string name;
    public string avatarUrlSmall;
    public string avatarUrlMedium;
    public string avatarUrlLarge;
}

[System.Serializable]
public struct LeaderboardYandex
{
    public LeaderboardData leaderboard;
    public LeaderboardEntries[] entries;
    public int userRank;
}

[System.Serializable]
public struct LeaderboardEntries
{
    public int score;
    public string extraData;
    public int rank;
    public LeaderboardPlayer player;
}

[System.Serializable]
public struct LeaderboardData
{
    public string appID;
    public string name;
}
[System.Serializable]
public struct LeaderboardPlayer
{
    public string publicName;
    public string lang;
    public string getAvatarSrc;
    public string uniqueID;
}
[System.Serializable]
public struct InapProduct
{
    public string id;
    public string title;
    public string description;
    public string imageURI;
    public string price;
    public string priceValue;
    public string priceCurrencyCode;
}
[System.Serializable]
public struct PurchaseItem
{
    public string productID;
    public string purchaseToken;
}
