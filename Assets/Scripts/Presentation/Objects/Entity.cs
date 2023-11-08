using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;


public class Entity : MonoBehaviour
{
    public EntityModel Model { get { return _model; } }



    [SerializeField] private EntityModel _model;
    
    private Color[] _colors = new Color[] { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow };
    private Color _color;
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
        SetRandomColorOnColoredComponents();        
    }


    private void OnCollisionEnter(Collision collision)
    {
        float collisionPrecision = 0.01f;
        if ((collision.GetContact(0).point - transform.position).normalized.y + 1 <= collisionPrecision)
        {     
            _rigidbody.isKinematic = true;            
            _collider.isTrigger = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        OnCollisionEnter(collision);
    }

    private void SetRandomColorOnColoredComponents()
    {
        _color = _colors[Random.Range(0, _colors.Length)];
        foreach (Renderer coloredComponent in _coloredComponents)
            coloredComponent.material.color = _color;
    }
}
