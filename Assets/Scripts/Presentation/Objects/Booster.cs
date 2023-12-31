using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Booster : MonoBehaviour, IBurnable
{
    public void Burn() { foreach (var item in _onBurning) item?.Invoke(); Destroy(this); Destroy(gameObject, 1f); }



    public virtual BoosterType GetBoosterType() => BoosterType.HEALTH_BOOSTER;

    public void SetOnBurning(Action onBurning) { _onBurning.Add(onBurning); }
    private List<Action> _onBurning = new List<Action>();

    private Rigidbody _rigidbody;    
    private Animator _animator;

    private bool _destroyingProcessIsStarted = false;
    private IEnumerator _destroyingProcess;




    





    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;



       
    }

    private void Start()
    {
        
        _animator.speed = UnityEngine.Random.Range(1f, 2f);
        transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359f), Vector3.up);
        StartCoroutine(CheckFallingStatus());


        
    }
    




    private void OnDestroy()
    {
        _onBurning.Clear();
    }


    private IEnumerator CheckFallingStatus()
    {
        const float FALLING_STATUS_CHECK_PERIOD = 1f, VERTICAL_VELOCITY_TRIGGER = -0.3f;       
        while (true)
        {
            if (_rigidbody == null)
            {
                if (_destroyingProcessIsStarted && _destroyingProcess != null) StopCoroutine(_destroyingProcess);
                yield break;
            }
            else
            {
                if (_rigidbody.velocity.y < VERTICAL_VELOCITY_TRIGGER)
                {
                    if (!_destroyingProcessIsStarted) { _destroyingProcess = DestroyingProcess(); StartCoroutine(_destroyingProcess); }
                }
                else if (_destroyingProcessIsStarted)
                {
                    _destroyingProcessIsStarted = false;
                    if (_destroyingProcess != null) StopCoroutine(_destroyingProcess);
                }
            }
            yield return new WaitForSeconds(FALLING_STATUS_CHECK_PERIOD);
        }
    }
    private IEnumerator DestroyingProcess()
    {
        const float MAX_FALLING_TIME = 10f;
        _destroyingProcessIsStarted = true;
        yield return new WaitForSeconds(MAX_FALLING_TIME);        
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_rigidbody == null) return;
        if (other.TryGetComponent(out Booster _) || other.TryGetComponent(out Entity _))
        {
            PhysicalRebound();
        }
        else if (!other.TryGetComponent(out Hunter _))
        {
            Destroy(_rigidbody);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_rigidbody == null) return;
        if (other.TryGetComponent(out Booster _) || other.TryGetComponent(out Entity _))
        {
            PhysicalRebound();
        }
    }

    private void PhysicalRebound()
    {
        float reboundForce = 0.5f;
        float verticalVelocityTrigger = 0.1f;
        if (_rigidbody.velocity.y > verticalVelocityTrigger) { return; }
        _rigidbody.AddForce((Vector3.up + Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359f), Vector3.up) * Vector3.forward) * reboundForce, ForceMode.Impulse);
    }

    
}
