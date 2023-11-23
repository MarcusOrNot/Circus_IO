using System.Collections;
using System;
using UnityEngine;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;
using TMPro;

[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour
{
    [Inject] private EntityFactory _factory;
    [Inject] private IEventBus _eventBus;
    

    public HunterModel Model { get => _model; }
    public bool IsReadyToBoost { get => _isBoosterReady; }
    public int Lifes { get => _health; }

    public void Boost() { StartCoroutine(BoosterActivation()); } 
    public void Move(Vector2 direction) { if (!_isMovingWithBoost) _character.GetMovingCommand(direction); }
    public void AddDamage(int value) { GetDamage(value); } 

    public void SetOnHealthChanged(Action<int> onHealthChanged) {  _onHealthChanged = onHealthChanged; }


    [SerializeField] private HunterModel _model;    
    private Character _character;        

    private bool _isBoosterReady = true;
    private bool _isMovingWithBoost = false;
    private bool _boosterIsReloading = false;

    private int _health = 0;
    private Action<int> _onHealthChanged;

    private bool _isGrowing = false;
    private IEnumerator _currentGrowingProcess = null;

    private EntityType _entityTypeForSpawn;
    //private int _maxEnititySpawnCountPerFrame = 10;    
    //private int _spawningEntitiesQueueSummaryHealth = 0;
    //private float _timeBetweenSpawnFrames = 0.05f;
        
    //private Renderer[] _visualComponents;
    //private Collider _collider;
    private Rigidbody _rigidbody;

    
    private void Awake()
    {
        _character = GetComponent<Character>();
        //_visualComponents = GetComponentsInChildren<Renderer>();
        //_collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {                
        _entityTypeForSpawn = (EntityType)Enum.GetValues(typeof(EntityType)).GetValue(0);
        //SetColorOnColoredComponents(_model.Color);
        transform.localScale = GetScaleDependingOnHealth(_model.StartEntity);
        ChangeHealth(_model.StartEntity); 
        SetBoostReady();

        //StartCoroutine(EntitySpawning());

        Material meshMaterial = GetComponentInChildren<SkinnedMeshRenderer>()?.materials?[0];
        if (meshMaterial != null) meshMaterial.color = _model.Color;
        INeedKaufmoColor[] coloredElements = GetComponentsInChildren<INeedKaufmoColor>();
        foreach (INeedKaufmoColor coloredElement in coloredElements) coloredElement.Color = _model.Color;   
    }
    private void OnDestroy()
    {        
        _eventBus?.NotifyObservers(GameEventType.HUNTER_DEAD);
    }

    /*
    private void SetColorOnColoredComponents(Color color)
    {
        foreach (Renderer coloredComponent in _model.ColoredComponents) 
            coloredComponent.material.color = color;
    }
    */

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
            _character.GetMovingCommand(new Vector2(transform.forward.x, transform.forward.z));
            yield return null;
        }  
    }    
    private void SetBoostReady() 
    { 
        _isBoosterReady = !_boosterIsReloading  && (_health > _model.BoostPrice); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            if (!entity.TryGetComponent(out Rigidbody _)) StartCoroutine(EatingSomebody(entity));                     
        }
    }
    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.TryGetComponent(out Hunter hunter))
        {
            if (_health > hunter.Lifes) StartCoroutine(EatingSomebody(hunter));                                
        }
    }

    private IEnumerator EatingSomebody(MonoBehaviour somebody) 
    {
        if (!(somebody is Entity) && !(somebody is Hunter)) { yield break; }
        float eatingSpeed = 10f;
        float objectDestroyingTime = 0;
        float monobehaviourDestroyingTime = 0;
        GameObject somebodyObject = somebody?.gameObject;
        Collider collider = somebodyObject?.GetComponent<Collider>();
        if (collider != null) Destroy(collider);
        if (somebody is Entity)
        {
            ChangeHealth((somebody as Entity).HealthCount);
            eatingSpeed = 7f * _character.SpeedMultiplier;
            objectDestroyingTime = 1f;
            monobehaviourDestroyingTime = 0;
        }
        else if (somebody is Hunter)
        {
            ChangeHealth((somebody as Hunter).Lifes);
            eatingSpeed = 7f * _character.SpeedMultiplier;
            objectDestroyingTime = 3f;
            monobehaviourDestroyingTime = 0.5f;
        }
        Destroy(somebody, monobehaviourDestroyingTime);
        Destroy(somebodyObject, objectDestroyingTime);        
        while (somebodyObject != null)
        {
            somebodyObject.transform.position = Vector3.Lerp(somebodyObject.transform.position, transform.position, eatingSpeed * Time.deltaTime);
            somebodyObject.transform.localScale = Vector3.Lerp(somebodyObject.transform.localScale, somebodyObject.transform.localScale / 10f, eatingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /*
    private void GetDamage(int value)
    {
        ChangeHealth(-value);
        if (_rigidbody == null) return;
        _spawningEntitiesQueueSummaryHealth += Math.Min(value, _health + value);
        if (_health <= 0) StartCoroutine(DestroyingAfterEntitySpawning());
    }
    */
    private void GetDamage(int value)
    {
        ChangeHealth(-value);
        if (_rigidbody == null) return;
        EntitySpawning(value / 5);        
        if (_health <= 0) Destroy(gameObject);
    }

    private void ChangeHealth(int value)
    {
        _health += value;
        _onHealthChanged?.Invoke(_health);
        SetBoostReady();
        if (_rigidbody == null) return;
        if (_model.IsScaleDependFromHealth) 
        { 
            if (_isGrowing) { StopCoroutine(_currentGrowingProcess); _isGrowing = false; }
            _currentGrowingProcess = GrowingProcess(GetScaleDependingOnHealth(_health));
            StartCoroutine(_currentGrowingProcess);
        }        
    }
    private Vector3 GetScaleDependingOnHealth(int health) 
    {
        return Vector3.one * (Mathf.Pow(Mathf.Max(10, health), 0.3f) / 1.5f); 
    }
    private IEnumerator GrowingProcess(Vector3 targetScale)
    {
        float growingSpeed = 0.01f;
        float timeBetweenGrow = 0.01f;
        _isGrowing = true;
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growingSpeed);
            yield return new WaitForSeconds(timeBetweenGrow);
        }
        _isGrowing = false;
    }





    private void EntitySpawning(int summaryHealth)
    {
        int entityCountForSpawn = Mathf.Min(5, summaryHealth);
        for (int i = entityCountForSpawn; i > 0; i--)
        {
            int currentEntityHealth = summaryHealth / i + (summaryHealth % i == 0 ? 0 : 1);
            SpawnOutEntity(currentEntityHealth);
            summaryHealth -= currentEntityHealth;
        }  
    }
    private Entity SpawnOutEntity(int entityHealth)
    {
        float pullOutForceUp = 5f;
        float pullOutForceHorizontal = 3f;
        float positionVerticalOffset = transform.localScale.x * 1f + 0.3f;
        float positionHorisontalOffset = transform.localScale.x * 1f + 0.3f;
        Vector3 horizontalDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(45, 315f), Vector3.up) * transform.forward;
        Entity spawnedEntity = _factory.Spawn(_entityTypeForSpawn);
        spawnedEntity.transform.position = transform.position + horizontalDirection * positionHorisontalOffset + Vector3.up * positionVerticalOffset;
        spawnedEntity.GetComponent<Rigidbody>().AddForce(Vector3.up * pullOutForceUp + horizontalDirection * pullOutForceHorizontal, ForceMode.Impulse);
        INeedKaufmoColor colorNeedable = spawnedEntity.GetComponentInChildren<INeedKaufmoColor>();
        if (colorNeedable != null) colorNeedable.Color = _model.Color;
        spawnedEntity.HealthCount = entityHealth;
        spawnedEntity.transform.localScale *= entityHealth switch
        {
            >= 1 and < 10 => 1f,
            >= 10 and < 50 => 1.5f,
            >= 50 and < 100 => 1.8f,
            >= 100 and < 500 => 2f,
            >= 500 => 2.2f,
            _ => 1f
        };  
        return spawnedEntity;
    }

    /*
    private IEnumerator EntitySpawning()
    {
        while (true)
        {
            int _spawnedEntitiesCount = 0;
            while ((_spawningEntitiesQueueSummaryHealth > 0) && (_spawnedEntitiesCount < _maxEnititySpawnCountPerFrame))            
            {
                _spawningEntitiesQueueSummaryHealth -= SpawnOutEntity().HealthCount;                
                _spawnedEntitiesCount++;
            }            
            _spawningEntitiesQueueSummaryHealth = Math.Max(0, _spawningEntitiesQueueSummaryHealth);            
            yield return new WaitForSeconds(_timeBetweenSpawnFrames);
        }
    }
    */
    /*
    private Entity SpawnOutEntity()
    {
        float pullOutForceUp = 5f;
        float pullOutForceHorizontal = 3f;
        float positionVerticalOffset = transform.localScale.x * 1f + 0.3f;
        float positionHorisontalOffset = transform.localScale.x * 1f + 0.3f;        
        Vector3 horizontalDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(45, 315f), Vector3.up) * transform.forward;        
        Entity spawnedEntity = _factory.Spawn(_entityTypeForSpawn);
        spawnedEntity.transform.position = transform.position + horizontalDirection * positionHorisontalOffset + Vector3.up * positionVerticalOffset;
        spawnedEntity.GetComponent<Rigidbody>().AddForce(Vector3.up * pullOutForceUp + horizontalDirection * pullOutForceHorizontal, ForceMode.Impulse);
        INeedKaufmoColor colorNeedable = spawnedEntity.GetComponentInChildren<INeedKaufmoColor>();
        if (colorNeedable != null) colorNeedable.Color = _model.Color;            
        return spawnedEntity;
    }
    */
    /*
    private IEnumerator DestroyingAfterEntitySpawning()
    {        
        Destroy(_rigidbody);
        Destroy(_collider);
        foreach (Renderer visualComponent in _visualComponents) { Destroy(visualComponent); }
        while (_spawningEntitiesQueueSummaryHealth > 0) { yield return null; }
        Destroy(gameObject);        
    }
    */

}
