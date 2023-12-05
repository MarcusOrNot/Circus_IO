using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdPauseMessage : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMessagePanel;
    [SerializeField] private TextMeshProUGUI _timeValueText;
    [SerializeField] private GameObject _pauseMenuPanel;
    private Action _onTimeFinished;
    public void StartPauseCounter(int secondsLeft)
    {
        _pauseMenuPanel.SetActive(false);
        _pauseMessagePanel.SetActive(false);
        StartCoroutine(TimeCounter(secondsLeft));
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
            if (current == 0) _onTimeFinished?.Invoke();
            current--;
            yield return new WaitForSeconds(1);
        }
    }

    public void SetOnTimeFinished(Action onTimeFinished)
    {
        _onTimeFinished = onTimeFinished;
    }
}
