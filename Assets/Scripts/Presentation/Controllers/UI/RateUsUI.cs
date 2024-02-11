using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RateUsUI : MonoBehaviour
{
    [SerializeField] private CountChoose _countArray;
    [SerializeField] private int _maxRate = 5;
    [SerializeField] private GameObject _ratePlace;
    [SerializeField] private GameObject _ratePanel;
    [SerializeField] private GameObject _feedPanel;
    [Inject] private IData _data;
    [Inject] private ISystemInfo _systemInfo;
    private void Awake()
    {
        /*_countArray.SetOnChoose((rate) =>
        {
            //Debug.Log("Rate is "+rate.ToString());
            if (rate > 3) ShowFeedPanel();
            else Close();
        });*/
        _countArray.SetCount(_maxRate);
    }

    private void OnEnable()
    {
        RuntimeInfo.IsShownRate = true;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ShowFeedPanel()
    {
        _ratePanel.SetActive(false);
        _feedPanel.SetActive(true);
    }

    public void OpenFeedPage()
    {
        Info.Analytics.LogFeedLeaving();
        Application.OpenURL(GameSettings.RATE_US_URL);
        Close();
    }

    public void Rate()
    {
        if (_countArray.Count == 0) return;
        Info.Analytics.LogRateChosen(_countArray.Count);
        _data.FeedValue = _countArray.Count;
        if (_countArray.Count > 3) ShowFeedPanel();
        else Close();
    }

    public void ShowByCondition1()
    {
        if (CanShowBySystem()==false) return;
        if (RuntimeInfo.IsShownRate == false && _data.FeedValue == 0)
            gameObject.SetActive(true);
    }

    public void ShowByCondtition2()
    {
        if (CanShowBySystem() == false) return;
        if (_data.FeedValue == 0 && RuntimeInfo.IsShownRate == false && RuntimeInfo.IsGamePlayedOnce == true)
            gameObject.SetActive(true);
    }

    private bool CanShowBySystem()
    {
        switch (_systemInfo.GetPlatformType())
        {
            case PlatformType.ANDROID:
                return true;
            case PlatformType.WEB_GL:
                break;
            case PlatformType.PC:
                break;
        }

        return false;
    }
}
