using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameUI
{
    public void SetLifesValue(int lifes);
    public void ShowWin();
    public void ShowGameOver();
}
