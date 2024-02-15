using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using DG.Tweening;

public class CoinsCounter : MonoBehaviour, IStatsObserver
{
    [SerializeField] private TextMeshProUGUI _coinsValueText;
    [Inject] private IGameStats _gameStats;
    private int _currentValue = 0;
    private void Awake()
    {
        Value = _gameStats.GetStat(GameStatsType.COINS);
        _gameStats.SetOnStatChanged(this);
    }
    private int Value
    {
        set
        {
            _currentValue = value;
            _coinsValueText.text = value.ToString();
        }
    }

    private void OnDestroy()
    {
        _gameStats.RemoveOnSettingChanged(this);
    }

    public void Notify(GameStatsType stat)
    {
        if (stat==GameStatsType.COINS)
        {
            //Value = _gameStats.GetStat(GameStatsType.COINS);
            CountAnimToValue(_gameStats.GetStat(GameStatsType.COINS));
        }
    }

    private void CountAnimToValue(int endValue)
    {
        int current = _currentValue;
        DOTween.To((x) =>
        {
            Value = Mathf.FloorToInt(x);
        },
        current, endValue, 2).OnComplete(() =>
        {
            Value = _currentValue;
        }).PlayForward();

    }
}
