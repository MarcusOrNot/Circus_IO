using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventBus
{
    void RegisterObserver(IGameEventObserver observer);
    void RemoveObserver(IGameEventObserver observer);
    void NotifyObservers(GameEventType eventType);
}
