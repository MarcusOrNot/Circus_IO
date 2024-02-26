using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillingButton : MonoBehaviour
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private RectTransform _buttonMask;
    private Sequence _buttonAnim;
    private Vector2 _maskSize;
    private Action _onClickAction;

    private void Awake()
    {
        _maskSize = _buttonMask.sizeDelta;
    }

    public void SetActionCooldown(float seconds)
    {
        if (_actionButton.gameObject.activeSelf == false) return;
        _buttonAnim.Kill();
        _buttonAnim = DOTween.Sequence();
        _buttonMask.sizeDelta = new Vector2(_maskSize.x, 0);
        //DOTween.To(y => _buttonMask.sizeDelta = new Vector2(_maskSize.x, y), 0, _maskSize.y, 5);
        _buttonAnim.Append(DOTween.To(y => _buttonMask.sizeDelta = new Vector2(_maskSize.x, y), 0, _maskSize.y, seconds));
        _buttonAnim.PlayForward();
    }

    public void SetOnClick(Action onClickAction)
    {
        _onClickAction = onClickAction;
    }

    public void Click()
    {
        if (_buttonAnim.IsActive()) return;
        _onClickAction?.Invoke();
    }

    private void OnEnable()
    {
        _actionButton.onClick.AddListener(() =>
        {
            Click();
        });
        
    }

    private void OnDisable()
    {
        _actionButton.onClick.RemoveAllListeners();
    }
}
