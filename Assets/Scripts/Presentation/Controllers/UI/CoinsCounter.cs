using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CoinsCounter : MonoBehaviour, IStatsObserver
{
    [SerializeField] private TextMeshProUGUI _coinsValueText;
    [Inject] private IGameStats _gameStats;
    private void Awake()
    {
        Value = _gameStats.GetStat(GameStatsType.COINS);
        _gameStats.SetOnStatChanged(this);
    }
    private int Value
    {
        set
        {
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
            Value = _gameStats.GetStat(GameStatsType.COINS);
        }
    }
}
