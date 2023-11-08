using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private IGameStats _stats;
    [Inject] private IEventBus _eventBus;
    [Inject] private EntityFactory _factory;
    // Start is called before the first frame update
    void Start()
    {
        _eventBus.NotifyObservers(GameEventType.HUNTER_SPAWNED);
        _factory.Spawn(EntityType.ENTITY1).transform.position = new Vector3(0,10,0);
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
