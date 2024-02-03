using System;
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
    [Inject] private IControlCharacter _controlUI;
    [Inject] private ILevelInfo _levelInfo;
    [Inject] private IMobSpawner _mobSpawner;
    
    void Start()
    {
        /*int level = GameStatService.GetLevel(301);
        Debug.Log("Level is "+ level.ToString());
        Debug.Log("Points is " + GameStatService.GetNeedExpByLevel(level+1).ToString());*/

        //_eventBus.NotifyObservers(GameEventType.HUNTER_SPAWNED);
        //_factory.Spawn(EntityType.ENTITY1).transform.position = new Vector3(0,10,0);
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
        Info.Analytics.LogLevelStarted();
        RuntimeInfo.IsGamePlayedOnce = true;
        //_mobSpawner.SpawnAtLocation(HunterType.HUNTER_BLACK, HatType.CAP, new Vector3(5,5,5));
        //Debug.Log("Now player us "+ Level.Instance.GetPlayer().GetPosition().ToString());
        //SetLevelParams(_levelProcessService.GenerateLevel(1));
        
        if (_levelInfo.GetLevelParams()!=null)
        {
            SetLevelParams(_levelInfo.GetLevelParams());
        }

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
                StartCoroutine(HunterCountHandler((count) =>
                {
                    GameOver();
                }));
                break;
            case GameEventType.HUNTER_DEAD:
                //StartCoroutine(CheckHuntersCount());
                StartCoroutine(HunterCountHandler((count) =>
                {
                    if (count==1 && Level.Instance.GetPlayer()!=null)
                    {
                        PlayerWon();
                    }
                }));
                break;
            case GameEventType.GAME_PAUSED:
                PauseGame();
                break;
            case GameEventType.GAME_CONTINUE:
                ResumeGame();
                break;
            case GameEventType.GAME_AD_PAUSED:
                PauseGame();
                Time.timeScale = 0;
                break;
        }
    }
    /*private IEnumerator CheckHuntersCount()
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
    }*/

    private IEnumerator HunterCountHandler(Action<int> onCount)
    {
        yield return new WaitForEndOfFrame();
        var count = FindObjectsOfType<Hunter>().Length;
        onCount?.Invoke(count);
        if (count<=1)
            Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        _music.Pause();
        _controlUI.Hide();
        //Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        _music.Continue();
        _controlUI.Show();
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Info.Analytics.LogLevelFailed();
        _gameUI.ShowGameOver();
        _music.Stop();
        _effect.PlayEffectConstantly(SoundEffectType.LEVEL_FAILED);
        /*_adService.ShowInterstitialIfAllowed((successfull) =>
        {
            _effect.PlayEffect(SoundEffectType.LEVEL_FAILED);
        });*/
    }

    public void PlayerWon()
    {
        //Time.timeScale = 0;
        Info.Analytics.LogLevelFinished();
        Level.Instance.GetDamageZone()?.Stop();
        PauseGame();
        _gameUI.ShowWin();
        _music.Stop();
        //_effect.PlayEffect(SoundEffectType.LEVEL_COMPLETED);
        _adService.ShowInterstitialIfAllowed((successfull) =>
        {
            _effect.PlayEffectConstantly(SoundEffectType.LEVEL_COMPLETED);
        });
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        _adService.HideBanner();
    }

    private void SetLevelParams(LevelParamsModel levelParams)
    {
        //Debug.Log("Now Generate coutn is "+levelParams.Mobs.Count.ToString());
        foreach(var mob in levelParams.Mobs)
        {
            //_mobSpawner.SpawnAtLocation(mob.HunterType, mob.HatType, Utils.GetRandomPlace(Vector3.zero, ) new Vector3(UnityEngine.Random.Range(1,10), 5, UnityEngine.Random.Range(1, 10)), mob.StartLifes);
            _mobSpawner.SpawnAtLocation(mob.HunterType, mob.HatType, Utils.GetRandomPlace(Vector3.zero, levelParams.MaxZoneSize), mob.StartLifes);
        }
    }
}
