using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusController : MonoBehaviour, IEventBus
{
    private List<IGameEventObserver> _observers = new List<IGameEventObserver>();

    public void NotifyObservers(GameEventType eventType)
    {
        foreach (var o in _observers)
        {
            o.Notify(eventType);
        }
    }

    public void RegisterObserver(IGameEventObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IGameEventObserver observer)
    {
        _observers.Remove(observer);
    }
}
