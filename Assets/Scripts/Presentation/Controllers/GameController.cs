using System.Collections;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour, IGameEventObserver
{
    [Inject] private IEventBus _eventBus;
    [Inject] private IGameUI _gameUI;
    [Inject] private IAudioEffect _effect;
    [Inject] private IMusicPlayer _music;
    [Inject] private IAds _ads;
    
    void Start()
    {
        //_eventBus.NotifyObservers(GameEventType.HUNTER_SPAWNED);
        //_factory.Spawn(EntityType.ENTITY1).transform.position = new Vector3(0,10,0);
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
        ResumeGame();
        
        
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
        switch(gameEvent)
        {
            case GameEventType.PLAYER_DEAD:
                PauseGame();
                _gameUI.ShowGameOver();
                _music.Stop();
                //_effect.PlayEffect(SoundEffectType.LEVEL_FAILED);
                _ads.ShowInterstitialAd((successfull) =>
                {
                    _effect.PlayEffect(SoundEffectType.LEVEL_FAILED);
                });
                break;
            case GameEventType.HUNTER_DEAD:
                StartCoroutine(CheckHuntersCount());
                break;
            case GameEventType.GAME_PAUSED:
                PauseGame();
                break;
            case GameEventType.GAME_CONTINUE:
                ResumeGame();
                break;
        }
    }
    private IEnumerator CheckHuntersCount()
    {
        for (int i = 0; i < 2; i++) yield return null;        
        var hunters = FindObjectsOfType<Hunter>();
        if (hunters.Length == 1)
        {
            if (hunters[0].GetComponent<PlayerHunter>() != null)
            {
                PauseGame();
                _gameUI.ShowWin();
                _music.Stop();
                _effect.PlayEffect(SoundEffectType.LEVEL_COMPLETED);
            }
        }
    }

    public void PauseGame()
    {
        _music.Pause();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        _music.Continue();
        Time.timeScale = 1f;
    }
}
