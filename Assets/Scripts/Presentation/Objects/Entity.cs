using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    

    private void Awake()
    {
        _coloredComponents = GetComponentsInChildren<Renderer>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359f), Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float collisionPrecision = 0.01f;
        if ((collision.GetContact(0).point - transform.position).normalized.y + 1 <= collisionPrecision)
        {
            if (collision.gameObject.TryGetComponent(out Hunter _))
            {
                _rigidbody.isKinematic = false;
                _collider.isTrigger = true;
            }
            else if (collision.gameObject.TryGetComponent(out Entity _))
            {
                _rigidbody.isKinematic = false;
                _collider.isTrigger = false;
                PhysicalRebound();
            }
            else
            {
                _rigidbody.isKinematic = true;
                _collider.isTrigger = true;
            }            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity _))
        {            
            _rigidbody.isKinematic = false;
            _collider.isTrigger = false;
            PhysicalRebound();
        }
    }
    
    private void PhysicalRebound()
    {
        float reboundForce = 0.5f;
        _rigidbody.AddForce((Vector3.up + Quaternion.AngleAxis(Random.Range(0, 359f), Vector3.up) * Vector3.forward) * reboundForce, ForceMode.Impulse);
    }

    private void SetColor(Color color)
    {
        foreach (Renderer coloredComponent in _coloredComponents)
            coloredComponent.material.color += color;
    }    

}
