using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Entity : MonoBehaviour, IBurnable
{
    public void Burn() { foreach (var item in _onBurning) item?.Invoke(); Destroy(this); Destroy(gameObject, 1f);}
    public void Eat() { foreach (var item in _onEating) item?.Invoke(); }

    public EntityModel Model { get => _model; }
    
    public int HealthCount { get => _model.HealCount; set { _model.HealCount = value; } }

    public void SetOnEating(Action onEating) { _onEating.Add(onEating); }
    private List<Action> _onEating = new List<Action>();

    public void SetOnBurning(Action onBurning) { _onBurning.Add(onBurning); }
    private List<Action> _onBurning = new List<Action>();



    [SerializeField] private EntityModel _model;  

    private Rigidbody _rigidbody; 
    
    
        
    private bool _destroyingProcessIsStarted = false;
    private IEnumerator _destroyingProcess;
    

    private void Awake()
    {        
        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
        
        
    }
    private void Start()
    {        
        transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359f), Vector3.up);
        
        StartCoroutine(CheckFallingStatus());

       
    }
   



    private void OnDestroy()
    {
        _onEating.Clear();
        _onBurning.Clear();
    }



    private void OnTriggerEnter(Collider other)
    {        
        if (_rigidbody == null) return;        
        if (other.TryGetComponent(out Entity _))
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
        if (other.TryGetComponent(out Entity _))
        {            
            PhysicalRebound();
        }
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

    private void PhysicalRebound()
    {        
        float reboundForce = 0.5f;
        float verticalVelocityTrigger = 0.1f;
        if (_rigidbody.velocity.y > verticalVelocityTrigger) { return; }        
        _rigidbody.AddForce((Vector3.up + Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359f), Vector3.up) * Vector3.forward) * reboundForce, ForceMode.Impulse);        
    }


    //public Color Color { set => SetColor(value); }

    //private Renderer[] _coloredComponents;

    //_coloredComponents = GetComponentsInChildren<Renderer>();

    /*
    private void Start()
    {
        transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359f), Vector3.up);
        transform.localScale = Vector3.one * (0.5f + Mathf.Min(_model.HealCount / 10f, 1f));   
        StartCoroutine(CheckFallingStatus());
    }
    */  
    /*
    private void SetColor(Color color)
    {
        foreach (Renderer coloredComponent in _coloredComponents) { coloredComponent.material.color += color; }            
    }
    */

}
