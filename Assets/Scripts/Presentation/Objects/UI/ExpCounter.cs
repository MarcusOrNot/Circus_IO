using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ExpCounter : MonoBehaviour, IStatsObserver
{
    [SerializeField] private TextMeshProUGUI _expValueText;
    [Inject] private IGameStats _gameStats;
    private void Awake()
    {
        Value = _gameStats.GetStat(GameStatsType.EXP);
        _gameStats.SetOnStatChanged(this);
    }
    public void Notify(GameStatsType stat)
    {
        if (stat == GameStatsType.EXP)
        {
            Value = _gameStats.GetStat(GameStatsType.EXP);
        }
    }

    private int Value
    {
        set
        {
            _expValueText.text = value.ToString();
        }
    }

    private void OnDestroy()
    {
        _gameStats.RemoveOnSettingChanged(this);
    }
}
