using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlCharacter
{
    public Vector2 Direction { get; }
    public void SetOnActionClicked(Action onClick);
}