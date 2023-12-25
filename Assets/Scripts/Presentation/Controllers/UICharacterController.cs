using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICharacterController : MonoBehaviour, IControlCharacter
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private Button _debafActionButton;
    [SerializeField] private RectTransform _buttonMask;
    [SerializeField] private Joystick _joystick;
    private Action _onClickAction;
    private Action _onDebafClickAction;
    private Sequence _buttonAnim;
    private Vector2 _maskSize;

    private void Awake()
    {
        _maskSize = _buttonMask.sizeDelta;
    }

    private void Start()
    {
        //SetActionCooldown(5);
    }

    public Vector2 Direction => _joystick.Direction;

    public void SetOnActionClicked(Action onClick)
    {
        _onClickAction = onClick;
    }

    public void SetOnDebafClicked(Action onClick)
    {
        _onDebafClickAction = onClick;
    }

    private void OnEnable()
    {
        _actionButton.onClick.AddListener(() =>
        {
            _onClickAction?.Invoke();
        });
        _debafActionButton.onClick.AddListener(() =>
        {
            _onDebafClickAction?.Invoke();
        });
    }

    private void OnDisable()
    {
        _actionButton.onClick.RemoveAllListeners();
        _debafActionButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            _onClickAction?.Invoke();
#endif
    }

    public void SetActionEnabled(bool enabled)
    {
        _buttonAnim.Kill();
        _actionButton.gameObject.SetActive(enabled);
        /*var tempColor = _actionButton.GetComponent<Image>().color;
        tempColor.a = enabled?1:0.3f;
        _actionButton.GetComponent<Image>().color = tempColor;*/
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

    public void Hide()
    {
        _actionButton.gameObject.SetActive(false);
        _debafActionButton.gameObject.SetActive(false);
        _joystick.gameObject.SetActive(false);
    }

    public void Show()
    {
        _actionButton.gameObject.SetActive(true);
        _debafActionButton.gameObject.SetActive(true);
        _joystick.gameObject.SetActive(true);
    }
}
