using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class AdPauseMessage : MonoBehaviour
{
    [Inject] private AdService _ad;
    //[SerializeField] private int _adDelay = 30;
    private int MIN_DELAY = 90;
    private int MAX_DELAY = 150;
    [SerializeField] private GameObject _pauseMessagePanel;
    [SerializeField] private TextMeshProUGUI _timeValueText;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private UnityEvent _onTimeFinished;
    [SerializeField] private UnityEvent _onContinue;

    private void Start()
    {
        StartPauseCounter();
    }

    public void StartPauseCounter()
    {
        Hide();
        StartCoroutine(TimeCounter(UnityEngine.Random.Range(MIN_DELAY, MAX_DELAY)));
    }

    private IEnumerator TimeCounter(int secondsLeft)
    {
        int current = secondsLeft;
        while (current >= 0)
        {
            if (current < 10)
            {
                _pauseMessagePanel.SetActive(true);
                _timeValueText.text = "The game will be paused after "+current.ToString();
            }
            if (current == 0)
            {
                Hide();
                _pauseMenuPanel.SetActive(true);
                _onTimeFinished?.Invoke();
            }
            current--;
            yield return new WaitForSeconds(1);
        }
    }

    /*public void SetOnTimeFinished(Action onTimeFinished)
    {
        _onTimeFinished = onTimeFinished;
    }*/

    public void Hide()
    {
        _pauseMenuPanel.SetActive(false);
        _pauseMessagePanel.SetActive(false);
    }

    public void StopMessage()
    {
        StopAllCoroutines();
        Hide();
    }

    public void ContinueWithAd()
    {
        //Debug.Log("Now should ad rewarded!");
        if (! _ad.ShowRewardedAd((successfull) =>
        {
            Hide();
            _onContinue?.Invoke();
        })) {
            Hide();
            _onContinue?.Invoke();
        }
    }
}
