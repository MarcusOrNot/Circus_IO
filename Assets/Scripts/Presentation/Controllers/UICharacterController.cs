using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterController : MonoBehaviour, IControlCharacter
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private Joystick _joystick;
    private Action _onClickAction;

    public Vector2 Direction => _joystick.Direction;

    public void SetOnActionClicked(Action onClick)
    {
        _onClickAction = onClick;
    }

    private void OnEnable()
    {
        _actionButton.onClick.AddListener(() =>
        {
            _onClickAction?.Invoke();
        });
    }

    private void OnDisable()
    {
        _actionButton.onClick.RemoveAllListeners();
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
        //_actionButton.gameObject.SetActive(enabled);
        var tempColor = _actionButton.GetComponent<Image>().color;
        tempColor.a = enabled?1:0.3f;
        _actionButton.GetComponent<Image>().color = tempColor;
    }
}
