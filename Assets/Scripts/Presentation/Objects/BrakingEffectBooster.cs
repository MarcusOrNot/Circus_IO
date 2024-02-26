using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class BrakingEffectBooster : MonoBehaviour
{
    [Inject] private EffectPlayService _effect;
    private AudioSource _audioSource;
    private const int DESTROY_DELAY_SECONDS = 5;
    private bool _isTriggered = false;
    private GameObject _parent = null;
    private List<IBrakableMoving> _brakes = new List<IBrakableMoving>();

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _effect.PlayEffect(SoundEffectType.BALL_INSTATIATE, _audioSource);
        StartCoroutine(DeleteCoroutine(DESTROY_DELAY_SECONDS));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTriggered==false)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _parent)
            AddBake(other.GetComponent<IBrakableMoving>());
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveBreak(other.GetComponent<IBrakableMoving>());
    }

    public void SetParentObject(GameObject parent)
    {
        _parent = parent;
    }

    private void OnDestroy()
    {
        foreach (var item in _brakes)
        {
            RemoveBreak(item);
        }
        _brakes.Clear();
    }

    private void RemoveBreak(IBrakableMoving brakeObject)
    {
        brakeObject?.BrakeOff();
        _brakes.Remove(brakeObject);
    }

    private void AddBake(IBrakableMoving brakeObject)
    {
        if (brakeObject!=null)
        {
            brakeObject.BrakeOn();
            _brakes.Add(brakeObject);
        }
    }

    private IEnumerator DeleteCoroutine(int delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        Destroy(this.gameObject);
    }
}
