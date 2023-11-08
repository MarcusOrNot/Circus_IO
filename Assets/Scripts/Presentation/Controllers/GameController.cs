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
        //_eventBus.NotifyObservers(GameEventType.HUNTER_SPAWNED);
        //_factory.Spawn(EntityType.ENTITY1).transform.position = new Vector3(0,10,0);
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
        for (int i=0; i < 10; i++)
        {
            _factory.Spawn(EntityType.ENTITY1).transform.position = new Vector3(5+i*3, 10, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
