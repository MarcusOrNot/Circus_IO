using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayController : MonoBehaviour, IMusicPlayer
{
    [SerializeField] private List<AudioClip> _musicClips;
    private AudioSource _musicSource;
    private int _currentMusicPos = 0;
    private void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayRandom();
    }

    /*public void Play(string musicName)
    {
        if (musicName != null)
        {
            Sound current = _musicClips.Find(m => m.name == musicName);
            if (current == null) return;
            _musicSource.clip = current.Clip;
            _musicSource.loop = current.Loop;
            _musicSource.volume = current.Volume;
        }
        //if (DataControl.Instance.Settings.Sound == false) return;
        if (_musicSource.clip != null) _musicSource.Play();
    }*/

    public void PlayRandom()
    {
        if (_musicClips.Count > 0)
        {
            PlayMusic(Random.Range(0, _musicClips.Count), 3);
        }
    }

    public void PlayMusic(int musicPos)
    {
        PlayMusic(musicPos, 0);
    }

    public void PlayMusic(int musicPos, int transitionSeconds)
    {
        Debug.Log("Now should play " + musicPos.ToString());
        _currentMusicPos = musicPos;
        var music = _musicClips[musicPos];
        _musicSource.clip = music;
        _musicSource.Play();
        if (transitionSeconds>0)
        {
            var currentVolume = _musicSource.volume;
            DOTween.To(() => 0, x => _musicSource.volume = x, currentVolume, transitionSeconds);
        }
    }

    /*public void Restart()
    {
        _musicSource.Stop();
        //Play(null);
        PlayRandom();
    }*/
    public void Stop()
    {
        _musicSource.Stop();
    }
    public void Pause()
    {
        //if (DataControl.Instance.Settings.Sound)
            _musicSource.Pause();
    }
    public void Continue()
    {
        //if (DataControl.Instance.Settings.Sound)
            _musicSource.Play();
    }
}
