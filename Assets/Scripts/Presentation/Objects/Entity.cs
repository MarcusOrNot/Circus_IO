using System.Collections;
using UnityEngine;


public class Entity : MonoBehaviour
{
    public EntityModel Model { get => _model; }
    public Color Color { set => SetColor(value); }
    public int HealthCount { get => _model.HealCount; }


    [SerializeField] private EntityModel _model;
    
        
    private Renderer[] _coloredComponents;

    private Rigidbody _rigidbody;
    private Collider _collider;

        
    private bool _destroyingProcessIsStarted = false;
    private IEnumerator _destroyingProcess;
    private float _maxFallingTime = 10f;


    private void Awake()
    {
        _coloredComponents = GetComponentsInChildren<Renderer>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
        _rigidbody.isKinematic = false;
        _collider.isTrigger = true;
    }
    private void Start()
    {        
        transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359f), Vector3.up);
        transform.localScale = Vector3.one * (0.5f + Mathf.Min(_model.HealCount / 10f, 1f));        
        StartCoroutine(CheckFallingStatus());        
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
        float fallingStatusCheckPeriod = 1f;
        float verticalVelocityTrigger = -0.3f;
        bool isFalling;  
        while (true)
        {            
            isFalling = (_rigidbody != null) && (_rigidbody.velocity.y < verticalVelocityTrigger);
            if (isFalling) 
            { 
                if (!_destroyingProcessIsStarted) { _destroyingProcess = DestroyingProcess(); StartCoroutine(_destroyingProcess); } 
            }
            else if (_destroyingProcessIsStarted) 
            {                
                _destroyingProcessIsStarted = false; 
                if (_destroyingProcess != null) StopCoroutine(_destroyingProcess); 
            }             
            yield return new WaitForSeconds(fallingStatusCheckPeriod);
        }  
    }
    private IEnumerator DestroyingProcess()
    {            
        _destroyingProcessIsStarted = true;        
        yield return new WaitForSeconds(_maxFallingTime);        
        Destroy(gameObject);
    }

    private void PhysicalRebound()
    {        
        float reboundForce = 0.5f;
        float verticalVelocityTrigger = 0.1f;
        if (_rigidbody.velocity.y > verticalVelocityTrigger) { return; }        
        _rigidbody.AddForce((Vector3.up + Quaternion.AngleAxis(Random.Range(0, 359f), Vector3.up) * Vector3.forward) * reboundForce, ForceMode.Impulse);        
    }

    private void SetColor(Color color)
    {
        foreach (Renderer coloredComponent in _coloredComponents) { coloredComponent.material.color += color; }            
    }
    
}
