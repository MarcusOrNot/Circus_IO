using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour, IGameEventObserver
{
    [Inject] private IEventBus _eventBus;
    [Inject] private IGameUI _gameUI;
    // Start is called before the first frame update
    void Start()
    {
        //_eventBus.NotifyObservers(GameEventType.HUNTER_SPAWNED);
        //_factory.Spawn(EntityType.ENTITY1).transform.position = new Vector3(0,10,0);
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _eventBus.RegisterObserver(this);
    }

    private void OnDisable()
    {
        _eventBus.RemoveObserver(this);
    }

    public void Notify(GameEventType gameEvent)
    {
        if (gameEvent==GameEventType.PLAYER_DEAD)
        {
            PauseGame();
            _gameUI.ShowGameOver();
        }
        else if (gameEvent==GameEventType.HUNTER_DEAD)
        {
            var hunters = FindObjectsOfType<Hunter>();
            if (hunters.Length == 1)
            {
                if (hunters[0].GetComponent<PlayerHunter>()!=null)
                {
                    PauseGame();
                    _gameUI.ShowWin();
                }
            }
        }
    }

    public void PauseGame()
    {
        Debug.Log("Now should pause the game!!!");
    }
}
