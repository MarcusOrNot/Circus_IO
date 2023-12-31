using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour, IGameEventObserver
{
    [Inject] private IEventBus _eventBus;
    [Inject] private IGameUI _gameUI;
    [Inject] private IAudioEffect _effect;
    [Inject] private IMusicPlayer _music;
    [Inject] private AdService _adService;
    
    void Start()
    {
        //_eventBus.NotifyObservers(GameEventType.HUNTER_SPAWNED);
        //_factory.Spawn(EntityType.ENTITY1).transform.position = new Vector3(0,10,0);
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
        Analytics.LogLevelStarted();
        RuntimeInfo.IsGamePlayedOnce = true;
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
                //PauseGame();
                GameOver();
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
            case GameEventType.GAME_AD_PAUSED:
                _music.Pause();
                Time.timeScale = 0;
                break;
        }
    }
    private IEnumerator CheckHuntersCount()
    {
        //for (int i = 0; i < 2; i++) yield return null;
        yield return new WaitForEndOfFrame();
        var hunters = FindObjectsOfType<Hunter>();
        if (hunters.Length == 1)
        {
            if (hunters[0].GetComponent<PlayerHunter>() != null)
            {
                PlayerWon();
            }
        }
    }

    public void PauseGame()
    {
        _music.Pause();
        //Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        _music.Continue();
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Analytics.LogLevelFailed();
        _gameUI.ShowGameOver();
        _music.Stop();
        _effect.PlayEffect(SoundEffectType.LEVEL_FAILED);
        /*_adService.ShowInterstitialIfAllowed((successfull) =>
        {
            _effect.PlayEffect(SoundEffectType.LEVEL_FAILED);
        });*/
    }

    public void PlayerWon()
    {
        //Time.timeScale = 0;
        Analytics.LogLevelFinished();
        Level.Instance.GetDamageZone()?.Stop();
        PauseGame();
        _gameUI.ShowWin();
        _music.Stop();
        //_effect.PlayEffect(SoundEffectType.LEVEL_COMPLETED);
        _adService.ShowInterstitialIfAllowed((successfull) =>
        {
            _effect.PlayEffect(SoundEffectType.LEVEL_COMPLETED);
        });
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        _adService.HideBanner();
    }
}
