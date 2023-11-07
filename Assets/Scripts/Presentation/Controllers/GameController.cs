using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private IGameStats _stats;
    [Inject] private IEventBus _eventBus;
    // Start is called before the first frame update
    void Start()
    {
        _eventBus.NotifyObservers(GameEventType.HUNTER_SPAWNED);
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
