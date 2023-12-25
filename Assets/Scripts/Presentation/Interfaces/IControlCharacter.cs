using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlCharacter
{
    public Vector2 Direction { get; }
    public void SetOnActionClicked(Action onClick);
    public void SetActionEnabled(bool enabled);
    public void SetActionCooldown(float seconds);
    public void SetOnDebafClicked(Action onClick);
    public void Hide();
}
