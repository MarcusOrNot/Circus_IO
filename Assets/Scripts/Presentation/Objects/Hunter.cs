using System.Collections;
using System;
using UnityEngine;
using Zenject;


[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour
{
    [Inject] private EntityFactory _factory;
    [Inject] private IEventBus _eventBus;
    

    public HunterModel Model { get => _model; }
    public bool IsReadyToBoost { get => _isBoosterReady; }
    public int Lifes { get => _health; }
    
    public void Boost() => StartCoroutine(BoosterActivation()); 
    public void Move(Vector2 direction) { if (!_isMovingWithBoost) _character.Move(direction); }
    public void AddDamage(int value) => GetDamage(value); 

    public void SetOnHealthChanged(Action<int> onHealthChanged) {  _onHealthChanged = onHealthChanged; }


    [SerializeField] private HunterModel _model;    
    private Character _character;
        

    private bool _isBoosterReady = true;
    private bool _isMovingWithBoost = false;
    private bool _boosterIsReloading = false;

    private int _health = 0;
    private Action<int> _onHealthChanged;

    private EntityType _entityTypeForSpawn;    

    private void Awake()
    {
        _character = GetComponent<Character>();        
    }
    private void Start()
    {        
        _entityTypeForSpawn = (EntityType)Enum.GetValues(typeof(EntityType)).GetValue(0);
        
        SetColorOnColoredComponents(_model.Color);
        ChangeHealth(_model.StartEntity);         
        
        SetBoostReady();        
    }
    private void OnDestroy()
    {
        _eventBus?.NotifyObservers(GameEventType.HUNTER_DEAD);
    }


    private void SetColorOnColoredComponents(Color color)
    {
        foreach (Renderer coloredComponent in _model.ColoredComponents) coloredComponent.material.color = color;
    }    
    
    private IEnumerator BoosterActivation()
    {
        if (!_isBoosterReady) yield break;
        StartCoroutine(BoosterReloading());
        _isMovingWithBoost = true;
        if (_health > _model.BoostPrice) GetDamage(_model.BoostPrice);  
        float savedBoostSpeed = _character.SpeedMultiplier;        
        _character.SpeedMultiplier = _model.BoostValue;
        StartCoroutine(MovingWithBoost());
        yield return new WaitForSeconds(_model.BoostTime);
        _character.SpeedMultiplier = savedBoostSpeed;
        _isMovingWithBoost = false;
    }
    private IEnumerator BoosterReloading()
    {
        _isBoosterReady = false;
        _boosterIsReloading = true;
        yield return new WaitForSeconds(_model.BoostRestartTime);
        _boosterIsReloading = false;
        SetBoostReady();
    }
    private IEnumerator MovingWithBoost()
    {
        while (_isMovingWithBoost)
        {
            _character.Move(new Vector2(transform.forward.x, transform.forward.z));
            yield return null;
        }  
    }
    
    private void SetBoostReady() { _isBoosterReady = !_boosterIsReloading  && (_health > _model.BoostPrice); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            ChangeHealth(entity.HealthCount);
            Destroy(entity.gameObject);            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.TryGetComponent(out Hunter hunter) && (_health > hunter.Lifes))
        { 
            ChangeHealth(hunter.Lifes);                
            Destroy(hunter.gameObject);                       
        }
    }
    

    private void ChangeHealth(int value)
    {
        _health += value;
        _onHealthChanged?.Invoke(_health);
        SetBoostReady();
        if (_model.IsScaleDependFromHealth) transform.localScale = Vector3.one * (1 + _health * 0.5f / 30);        
    }

    private void GetDamage(int value)
    {
        ChangeHealth(-value);        
        SpawnOutEntities(Mathf.Min(value, _health + value));
        if (_health <= 0) Destroy(gameObject);        
    }

    private void SpawnOutEntities(int summaryHealth)
    {
        while (summaryHealth > 0) { summaryHealth -= SpawnOutEntity().HealthCount; }
    }

    private Entity SpawnOutEntity()
    {
        float pullOutForceUp = 5f;
        float pullOutForceHorizontal = 3f;
        float positionVerticalOffset = transform.localScale.x * 1f;
        float positionHorisontalOffset = transform.localScale.x * 1f;        
        Vector3 horizontalDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(45, 315f), Vector3.up) * transform.forward;        
        Entity spawnedEntity = _factory.Spawn(_entityTypeForSpawn);
        spawnedEntity.transform.position = transform.position + horizontalDirection * positionHorisontalOffset + Vector3.up * positionVerticalOffset;
        spawnedEntity.GetComponent<Rigidbody>().AddForce(Vector3.up * pullOutForceUp + horizontalDirection * pullOutForceHorizontal, ForceMode.Impulse);        
        spawnedEntity.Color = _model.Color;
        return spawnedEntity;
    }


    
}
