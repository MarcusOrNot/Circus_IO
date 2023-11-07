using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEventObserver
{
    void Notify(GameEventType gameEvent);
}
