using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ExpCounter : MonoBehaviour, IStatsObserver
{
    [SerializeField] private TextMeshProUGUI _expValueText;
    [SerializeField] private TextMeshProUGUI _levelValueText;
    [SerializeField] private RectTransform _progressTransform;
    [Inject] private IGameStats _gameStats;
    [Inject] private GameStatService _gameStatService;
    //private int _currentExp = 0;
    private int _targetExp = 0;
    private float _startSize;
    private void Awake()
    {
        _startSize = GetComponent<RectTransform>().sizeDelta.x; //_progressTransform.sizeDelta;
        //Debug.Log(_startSize.ToString());
        //_progressTransform.sizeDelta = new Vector2(_startSize.x/2, _startSize.y);
        //_currentExp = _gameStatService.GetCurrentLevel();
        Value = _gameStats.GetStat(GameStatsType.EXP);
        //_levelValueText.text = _gameStatService.GetCurrentLevel().ToString();
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
            //_currentExp = value;
            //_expValueText.text = value.ToString();
            int level = GameStatService.GetLevel(value);
            if (value>= _targetExp)
            {
                _levelValueText.text = level.ToString();
                _targetExp = GameStatService.GetTargetExp(level+1);
            }
            _expValueText.text = value.ToString()+" / "+_targetExp.ToString();
            int baseLevelExp = GameStatService.GetTargetExp(level);
            var currentSize = ((float) value - baseLevelExp) / (_targetExp - baseLevelExp) * _startSize;
            //Debug.Log(targetKoef.ToString());
            _progressTransform.sizeDelta = new Vector2(-(_startSize - currentSize), 0);
            //Debug.Log("Count of level " + level.ToString() + ", " + _targetExp.ToString());
        }
    }

    /*private void SetExpData(int currentExp, int )
    {

    }*/

    private void OnDestroy()
    {
        _gameStats.RemoveOnSettingChanged(this);
    }
}
