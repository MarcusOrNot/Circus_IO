using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class EffectPlayController : MonoBehaviour, IAudioEffect
{
    [Inject] private EffectPlayService _effectPlayService;
    private AudioSource _audioSource;
    private bool _isBlocked = false;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void PlayEffect(SoundEffectType effect)
    {
        if (_isBlocked == false)
        {
            _effectPlayService.PlayEffect(effect, _audioSource);
        }
    }
    public void PlayEffectConstantly(SoundEffectType effect)
    {
        PlayEffect(effect);
        _isBlocked = true;
    }
    private void Update()
    {
        if (_isBlocked == true) {
            if (_audioSource.isPlaying == false)
                _isBlocked = false;
        }
    }
}
