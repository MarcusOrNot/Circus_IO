using System.Collections;
using System;
using UnityEngine;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;
using TMPro;
using System.Collections.Generic;
using UnityEngine.ProBuilder;
using Unity.VisualScripting;

[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour, IBurnable
{
    public void Burn()
    {

    }

    [Inject] private EntityFactory _factory;
    [Inject] private IEventBus _eventBus;

    public HunterModel Model { get => _model; }
    public int Lifes { get => _health; }
    public void Boost() => StartCoroutine(BoosterActivation());  
    public void Move(Vector2 direction) { if (!_isMovingWithBoost) _character.GetMovingCommand(direction); }
    public void AddDamage(int value) => GetDamage(value);
    public bool KaufmoIsActive { get => _kaufmoIsActivated; }


    public void SetOnHealthChanged(Action<int> onHealthChanged) {  _onHealthChanged.Add(onHealthChanged); }
    private List<Action<int>> _onHealthChanged = new List<Action<int>>();

    public void SetOnHunterModeChanged(Action<bool> onKaufmoActivated) { _onKaufmoActivated.Add(onKaufmoActivated); }
    private List<Action<bool>> _onKaufmoActivated = new List<Action<bool>>();

    public void SetOnBoostStateChanged(Action<bool> onBoostStateChanged) { _onBoostStateChanged = onBoostStateChanged; }
    private Action<bool> _onBoostStateChanged;

    public void SetOnBoostingStateChanged(Action<bool> onBoostingStateChanged) { _onBoostingStateChanged = onBoostingStateChanged; }
    private Action<bool> _onBoostingStateChanged;

    public void SetOnScaleChanged(Action<Vector3> onScaleChanged) { _onScaleChanged = onScaleChanged; } 
    private Action<Vector3> _onScaleChanged;

        
    [SerializeField] private HunterModel _model;

    private Character _character;
    
    private Rigidbody _rigidbody;
    private EffectPlayController _soundEffectsController;
     
    private bool _isBoosterReady = true;
    private bool _isMovingWithBoost = false;
    private bool _boosterIsReloading = false;

    private int _health = 0; 

    private bool _isGrowing = false;
    private IEnumerator _currentGrowingProcess = null; 
    private const float MAX_SCALE = 7f;

    private bool _kaufmoIsActivated = false;
    [SerializeField] GameObject _kaufmo;
    [SerializeField] GameObject _bubble;

    private float _defaultCharacterSpeedMultiplier = -1f;


    private void Awake()
    {        
        _character = GetComponent<Character>();  
        _rigidbody = GetComponent<Rigidbody>();
        _soundEffectsController = GetComponent<EffectPlayController>();
    }

    
    
    private void Start()
    {
        _defaultCharacterSpeedMultiplier = Mathf.Max(_defaultCharacterSpeedMultiplier, _character.SpeedMultiplier);
        
        transform.localScale = GetScaleDependingOnHealth(_model.StartHealth);        
        ChangeHealth(_model.StartHealth); 

        Material meshMaterial = _kaufmo?.GetComponentInChildren<SkinnedMeshRenderer>(true)?.materials?[0];
        if (meshMaterial != null) meshMaterial.color = _model.Color;
        INeedKaufmoColor[] coloredElements = _kaufmo?.GetComponentsInChildren<INeedKaufmoColor>(true);
        foreach (INeedKaufmoColor coloredElement in coloredElements) coloredElement.Color = _model.Color;
        
    }

    private void OnDestroy()
    {        
        _eventBus?.NotifyObservers(GameEventType.HUNTER_DEAD);
        _onHealthChanged.Clear();     
        _onKaufmoActivated.Clear();
    }    
    

    private IEnumerator BoosterActivation()
    {
        if (!_isBoosterReady) yield break;
        StartCoroutine(BoosterReloading());
        _isMovingWithBoost = true;  _onBoostingStateChanged?.Invoke(_isMovingWithBoost);
        _soundEffectsController?.PlayEffect(SoundEffectType.HUNTER_ACCELERATED);  
        if (_health > _model.BoostPrice) GetDamage(_model.BoostPrice);  
        float savedBoostSpeed = _character.SpeedMultiplier;        
        _character.SpeedMultiplier = _model.BoostSpeedMultiplier;
        StartCoroutine(MovingWithBoost());
        yield return new WaitForSeconds(_model.BoostTime);
        if (!_kaufmoIsActivated)
        {
            _character.SpeedMultiplier = savedBoostSpeed;
            _isMovingWithBoost = false; _onBoostingStateChanged?.Invoke(_isMovingWithBoost);
        }        
    }
    private IEnumerator BoosterReloading()
    {        
        _boosterIsReloading = true;
        SetBoostReadyState();
        yield return new WaitForSeconds(_model.BoostRestartTime);
        if (!_kaufmoIsActivated)
        {
            _boosterIsReloading = false;
            SetBoostReadyState();
        }            
    }
    private IEnumerator MovingWithBoost()
    {
        while (_isMovingWithBoost && !_kaufmoIsActivated)
        {
            _character.GetMovingCommand(new Vector2(transform.forward.x, transform.forward.z));
            yield return null;
        }  
    }    
    private void SetBoostReadyState() 
    {
        _isBoosterReady = !_kaufmoIsActivated && !_boosterIsReloading && (_health > _model.BoostPrice); 
        _onBoostStateChanged?.Invoke(_isBoosterReady);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!_kaufmoIsActivated)
        {
            if (other.TryGetComponent(out Entity entity) && !entity.TryGetComponent(out Rigidbody _)) StartCoroutine(EatingSomebody(entity));
            else if (other.TryGetComponent(out Booster booster) && !booster.TryGetComponent(out Rigidbody _)) StartCoroutine(EatingSomebody(booster));
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_kaufmoIsActivated)
        {
            if (collision.gameObject.TryGetComponent(out Hunter hunter) && (!hunter.KaufmoIsActive) && (_health > hunter.Lifes)) StartCoroutine(EatingSomebody(hunter));
        }                   
    }
    private IEnumerator EatingSomebody(MonoBehaviour somebody) 
    {        
        if (_kaufmoIsActivated || (!(somebody is Entity) && !(somebody is Hunter) && !(somebody is Booster))) { yield break; }
        float eatingSpeed = 10f, objectDestroyingTime = 0, monobehaviourDestroyingTime = 0;
        GameObject somebodyObject = somebody?.gameObject;
        Collider collider = somebodyObject?.GetComponent<Collider>();
        if (collider != null) Destroy(collider);
        switch (somebody)
        {
            case Entity _:
                ChangeHealth((somebody as Entity).HealthCount);
                eatingSpeed = 7f * _character.SpeedMultiplier;
                objectDestroyingTime = 1f;
                monobehaviourDestroyingTime = 0;
                _soundEffectsController?.PlayEffect(SoundEffectType.ENTITY_EAT);
                break;                
            case Hunter _:
                ChangeHealth((somebody as Hunter).Lifes);
                eatingSpeed = 7f * _character.SpeedMultiplier;
                objectDestroyingTime = 0.3f;
                monobehaviourDestroyingTime = 0.5f;
                break;
            case Booster _:
                switch ((somebody as Booster).BoosterModel.BoosterType)
                {
                    case BoosterType.HEALTH_BOOSTER:
                        ChangeHealth((somebody as Booster).BoosterModel.HealCount);
                        break;
                    case BoosterType.KAUFMO_CONVERTER:
                        StartCoroutine(ActivateKaufmoMode((somebody as Booster).BoosterModel.KaufoModeTime));
                        break;
                } 
                eatingSpeed = 7f * _character.SpeedMultiplier;
                objectDestroyingTime = 0.3f;
                monobehaviourDestroyingTime = 0.5f;
                break;
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

    
    private void GetDamage(int value)
    {
        ChangeHealth(-value);        
        if (_health <= 0) Destroy(gameObject);
    }
    private void ChangeHealth(int value)
    {
        _health += value; foreach (var item in _onHealthChanged) item.Invoke(_health); 
        SetBoostReadyState();
        if (_model.IsScaleDependFromHealth && _rigidbody != null)
        {
            if (_isGrowing) { StopCoroutine(_currentGrowingProcess); _isGrowing = false; }
            _currentGrowingProcess = GrowingProcess(GetScaleDependingOnHealth(_health));
            StartCoroutine(_currentGrowingProcess);
        }         
    }
    private Vector3 GetScaleDependingOnHealth(int health) 
    {
        return Vector3.one * Mathf.Min(Mathf.Pow(Mathf.Max(10, health), 0.3f) / 1.5f, MAX_SCALE); 
    }

    private IEnumerator GrowingProcess(Vector3 targetScale)
    {
        const float GROWING_SPEED = 0.01f, TIME_BETWEEN_GROW_FRAMES = 0.01f;        
        _isGrowing = true;
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, GROWING_SPEED);                        
            yield return new WaitForSeconds(TIME_BETWEEN_GROW_FRAMES);
        }
        _onScaleChanged?.Invoke(transform.localScale);
        _isGrowing = false;
    }

    private IEnumerator ActivateKaufmoMode(float time)
    {
        _kaufmoIsActivated = true; foreach (var item in _onKaufmoActivated) item.Invoke(_kaufmoIsActivated);
        SetBoostReadyState();
        _boosterIsReloading = false;
        _isMovingWithBoost = false; _onBoostingStateChanged?.Invoke(_isMovingWithBoost);
        _kaufmo?.SetActive(true); 
        _bubble?.SetActive(false);
        _character.ChangeAnimator(); _character.SpeedMultiplier = _model.KaufmoSpeedMultiplier;
        yield return new WaitForSeconds(time);
        _bubble?.SetActive(true);
        _kaufmo?.SetActive(false);
        _character.ChangeAnimator(); _character.SpeedMultiplier = _defaultCharacterSpeedMultiplier;
        _kaufmoIsActivated = false; foreach (var item in _onKaufmoActivated) item.Invoke(_kaufmoIsActivated);
        SetBoostReadyState();        
    }



    // private EntityType _entityTypeForSpawn;
    // IN "START"  _entityTypeForSpawn = (EntityType)Enum.GetValues(typeof(EntityType)).GetValue(0); 
    // IN "GET_DAMAGE"     if (_rigidbody != null) EntitySpawning(value / 5); 
    /*
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
    */


    //private int _maxEnititySpawnCountPerFrame = 10;    
    //private int _spawningEntitiesQueueSummaryHealth = 0;
    //private float _timeBetweenSpawnFrames = 0.05f;
    //private Renderer[] _visualComponents;
    //private Collider _collider;
    //_visualComponents = GetComponentsInChildren<Renderer>();
    //_collider = GetComponent<Collider>();

    //SetColorOnColoredComponents(_model.Color);
    //StartCoroutine(EntitySpawning());

    /*
    private void SetColorOnColoredComponents(Color color)
    {
        foreach (Renderer coloredComponent in _model.ColoredComponents) 
            coloredComponent.material.color = color;
    }
    */
    /*
    private void GetDamage(int value)
    {
        ChangeHealth(-value);
        if (_rigidbody == null) return;
        _spawningEntitiesQueueSummaryHealth += Math.Min(value, _health + value);
        if (_health <= 0) StartCoroutine(DestroyingAfterEntitySpawning());
    }
    */
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



