using System.Collections;
using System;
using UnityEngine;
using Zenject;
using System.Collections.Generic;


[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour, IBurnable, IBrakableMoving
{      
    public HunterModel Model { get => _model; }
    public int Lifes { get => _health; }
    public bool KaufmoIsActive { get => _kaufmoIsActivated; }
    
    public void Boost() => StartCoroutine(BoosterActivation());
    
    public void SpawnDebaff() 
    {
        if (!_debafferSpawnerIsReady) { Debug.Log("debafferSpawner is reloading"); return; }
        if (_debaffersCount <= 0) { Debug.Log("debaffers are out"); return; }        
        _debaffersCount--;
        StartCoroutine(DebafferSpawnerReloading());
        var debaf = _brakingFactory.Spawn(this.gameObject);
        debaf.transform.position = transform.position - transform.forward * 3;
    }

    public void Move(Vector2 direction) { if (!_isMovingWithBoost) _character.GetMovingCommand(direction); }
    public void AddDamage(int value) => GetDamage(value);
    public void Burn() { GetDamage(Mathf.Max(100, _health / 2)); foreach (var item in _onBurning) item?.Invoke(); }    
    
    public void SetHat(HatType hatType) 
    {
        if (_character.MainForm.TryGetComponent(out ILoveHat mainHatLover)) { mainHatLover.SetHat(hatType); }
        if (_character.SecondForm.TryGetComponent(out ILoveHat secondHatLover)) { secondHatLover.SetHat(hatType); }
    }
      

    public void SetOnHealthChanged(Action<int> onHealthChanged) { _onHealthChanged.Add(onHealthChanged); }
    private List<Action<int>> _onHealthChanged = new List<Action<int>>();

    public void SetOnHunterModeChanged(Action<bool> onKaufmoActivated) { _onKaufmoActivated.Add(onKaufmoActivated); }
    private List<Action<bool>> _onKaufmoActivated = new List<Action<bool>>();

    public void SetOnBoostStateChanged(Action<bool> onBoostStateChanged) { _onBoostStateChanged.Add(onBoostStateChanged); }
    private List<Action<bool>> _onBoostStateChanged = new List<Action<bool>>();

    public void SetOnBoostingStateChanged(Action<bool> onBoostingStateChanged) { _onBoostingStateChanged.Add(onBoostingStateChanged); }
    private List<Action<bool>> _onBoostingStateChanged = new List<Action<bool>>();

    public void SetOnScaleChanged(Action<Vector3> onScaleChanged) { _onScaleChanged.Add(onScaleChanged); }
    private List<Action<Vector3>> _onScaleChanged = new List<Action<Vector3>>();

    public void SetOnBurning(Action onBurning) { _onBurning.Add(onBurning); }
    private List<Action> _onBurning = new List<Action>();

    public void SetOnDestroying(Action onDestroying) { _onDestroying.Add(onDestroying); }
    private List<Action> _onDestroying = new List<Action>();

    public void SetOnDebaffChanged(Action<bool, int> onDebaffChanged) { _onDebaffChanged.Add(onDebaffChanged); }
    private List<Action<bool, int>> _onDebaffChanged = new();



    [Inject] private IEventBus _eventBus;
    [Inject] private BrakingObjectFactory _brakingFactory;
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

    private float _defaultCharacterSpeedMultiplier = -1f;
    private bool _speedIsDebaffed = false;
    private int _debaffersCount = 0;

    private Hunter _currentCollidedKaufmo = null;
    private float _collidedKaufmoDamagingPeriod = 1f;

    private bool _debafferSpawnerIsReady = true;
        
    private void Awake()
    {        
        _character = GetComponent<Character>();  
        _rigidbody = GetComponent<Rigidbody>();
        _soundEffectsController = GetComponent<EffectPlayController>();  
    } 
    private void Start()
    {        
        _character.SecondForm.SetVisiblityStatus(false);
        _defaultCharacterSpeedMultiplier = Mathf.Max(_defaultCharacterSpeedMultiplier, _character.SpeedMultiplier);        
        transform.localScale = GetScaleDependingOnHealth(_model.StartHealth);        
        ChangeHealth(_model.StartHealth);
        if (_character.MainForm.TryGetComponent(out ICanAttack mainAttacker)) mainAttacker.SetOnAttack(() => AttackCurrentCollidedHunter());
        if (_character.SecondForm.TryGetComponent(out ICanAttack secondAttacker)) secondAttacker.SetOnAttack(() => AttackCurrentCollidedHunter());
        _debaffersCount = _model.StartDebaffersCount;   
        //Array hatTypes = Enum.GetValues(typeof(HatType)); SetHat((HatType)hatTypes.GetValue(UnityEngine.Random.Range(0, hatTypes.Length))); //ТЕСТ: случайно надевает шапки в начале                                                                                                                                          
    }
    private void OnDestroy()
    {        
        
        _onHealthChanged.Clear();     
        _onKaufmoActivated.Clear();
        _onScaleChanged.Clear();
        _onBoostingStateChanged.Clear();
        _onBoostStateChanged.Clear();
        _onBurning.Clear();
        _onDestroying.Clear();
        _onDebaffChanged.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_kaufmoIsActivated)
        {
            if (other.TryGetComponent(out Entity entity) && !entity.TryGetComponent(out Rigidbody _)) 
                StartCoroutine(EatingSomebody(entity));
            else if (other.TryGetComponent(out Booster booster) && !booster.TryGetComponent(out Rigidbody _)) 
                StartCoroutine(EatingSomebody(booster));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_kaufmoIsActivated)
        {
            if (collision.gameObject.TryGetComponent(out Hunter hunter) && (!hunter.KaufmoIsActive) && (_health > hunter.Lifes)) StartCoroutine(EatingSomebody(hunter));
        }
        else
        {
            if (collision.gameObject.TryGetComponent(out Hunter hunter) && (!hunter.KaufmoIsActive) && (_currentCollidedKaufmo == null))
            {
                _currentCollidedKaufmo = hunter;
                StartCoroutine(KaufmoDamaging(hunter));
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        OnCollisionEnter(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Hunter hunter) && (hunter == _currentCollidedKaufmo))  _currentCollidedKaufmo = null; 
    }


    private IEnumerator BoosterActivation()
    {
        if (!_isBoosterReady) yield break;
        StartCoroutine(BoosterReloading());
        _isMovingWithBoost = true; foreach (var item in _onBoostingStateChanged) item?.Invoke(_isMovingWithBoost);
        _soundEffectsController?.PlayEffect(SoundEffectType.HUNTER_ACCELERATED);  
        if (_health > _model.BoostPrice) GetDamage(_model.BoostPrice);
        ChangeCharacterSpeed();
        StartCoroutine(MovingWithBoost());
        yield return new WaitForSeconds(_model.BoostTime);
        if (_isMovingWithBoost)
        {
            _isMovingWithBoost = false;
            ChangeCharacterSpeed();
            foreach (var item in _onBoostingStateChanged) item?.Invoke(_isMovingWithBoost);
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
        bool isBoosterReady = !_kaufmoIsActivated && !_boosterIsReloading && (_health > _model.BoostPrice);
        if (_isBoosterReady != isBoosterReady)
        {
            _isBoosterReady = isBoosterReady;
            foreach (var item in _onBoostStateChanged) item?.Invoke(_isBoosterReady);
        }          
    }

        
    private IEnumerator KaufmoDamaging(Hunter hunter)
    {
        while ((hunter != null) && !hunter.KaufmoIsActive && (_currentCollidedKaufmo == hunter))
        {
            _character.StartAttackAnimation();            
            yield return new WaitForSeconds(_collidedKaufmoDamagingPeriod);
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
                (somebody as Entity).Eat();
                break;                
            case Hunter _:
                ChangeHealth((somebody as Hunter).Lifes);
                eatingSpeed = 7f * _character.SpeedMultiplier;
                objectDestroyingTime = 0.3f;
                monobehaviourDestroyingTime = 0.5f;
                (somebody as Hunter).AddDamage((somebody as Hunter).Lifes + 100);
                break;
            case Booster _:
                switch ((somebody as Booster).GetBoosterType())
                {
                    case BoosterType.HEALTH_BOOSTER:
                        ChangeHealth((somebody as HealthBooster).HealCount);
                        break;
                    case BoosterType.KAUFMO_CONVERTER:
                        KaufmoActivatorBooster kaufmoActivatorBooster = (somebody as KaufmoActivatorBooster);
                        StartCoroutine(ActivateKaufmoMode(kaufmoActivatorBooster.Model.KaufoModeTime, kaufmoActivatorBooster.Model.FlickingTimeFraction, kaufmoActivatorBooster.Model.FlickingSpeed));
                        break;
                } 
                eatingSpeed = 7f * _character.SpeedMultiplier;
                objectDestroyingTime = 0.3f;
                monobehaviourDestroyingTime = 0.5f;
                _soundEffectsController?.PlayEffect(SoundEffectType.BOOSTER_EAT);
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
        
    }
    private void ChangeHealth(int value)
    {
        _health += value; foreach (var item in _onHealthChanged) item.Invoke(_health); 
        SetBoostReadyState();

        if (_health <= 0) 
        { 
            foreach (var item in _onDestroying) item?.Invoke();
            _eventBus?.NotifyObservers(GameEventType.HUNTER_DEAD);
            Destroy(gameObject); 
        }
        

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
        const int SCALE_INFORMATION_SENDER_PERIOD_IN_FRAMES = 60;
        int frameCounter = SCALE_INFORMATION_SENDER_PERIOD_IN_FRAMES;
        _isGrowing = true;
        while (transform.localScale != targetScale)
        {            
            if (frameCounter < 0) 
            { 
                frameCounter = SCALE_INFORMATION_SENDER_PERIOD_IN_FRAMES; 
                foreach (var item in _onScaleChanged) item?.Invoke(transform.localScale); 
            }
            frameCounter--;
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, GROWING_SPEED);                        
            yield return new WaitForSeconds(TIME_BETWEEN_GROW_FRAMES);
        }        
        foreach (var item in _onScaleChanged) item?.Invoke(transform.localScale);
        _isGrowing = false;
    }    

    
    private IEnumerator ActivateKaufmoMode(float time, float flickingTimeFraction, float flickingSpeed )
    {        
        float flickingTime = time * flickingTimeFraction;
        _kaufmoIsActivated = true; foreach (var item in _onKaufmoActivated) item?.Invoke(_kaufmoIsActivated);
        SetBoostReadyState();
        _boosterIsReloading = false;
        _isMovingWithBoost = false; foreach (var item in _onBoostingStateChanged) item?.Invoke(_isMovingWithBoost); 
        _character.SecondForm.SetVisiblityStatus(true);
        _character.MainForm.SetVisiblityStatus(false);
        ChangeCharacterSpeed();
        yield return new WaitForSeconds(time - flickingTime);
        StartCoroutine(HunterModelFlicking(flickingTime, flickingSpeed));
        yield return new WaitForSeconds(flickingTime);
        ChangeCharacterSpeed();
        _character.MainForm.SetVisiblityStatus(true);
        _character.SecondForm.SetVisiblityStatus(false);
        _kaufmoIsActivated = false; foreach (var item in _onKaufmoActivated) item.Invoke(_kaufmoIsActivated);
        SetBoostReadyState();        
    }
    private IEnumerator HunterModelFlicking(float flickingTime, float flickingSpeed)
    {        
        bool mainFormIsVisible = false;
        while ((flickingTime > 0) && _kaufmoIsActivated)
        {
            mainFormIsVisible = !mainFormIsVisible;
            _character.MainForm.SetVisiblityStatus(mainFormIsVisible);
            _character.SecondForm.SetVisiblityStatus(!mainFormIsVisible);            
            yield return new WaitForSeconds(flickingSpeed);
            flickingTime -= flickingSpeed;            
        }        
        _character.SecondForm.SetVisiblityStatus(false);
        _character.MainForm.SetVisiblityStatus(true);
    }


    private void AttackCurrentCollidedHunter()
    {
        if (_currentCollidedKaufmo != null)
        {
            _currentCollidedKaufmo.AddDamage(Mathf.Max(100, _currentCollidedKaufmo.Lifes / 2));
            _soundEffectsController?.PlayEffect(SoundEffectType.MELEE_ATTACK);
        }
    }

    public void BrakeOn()
    {
        Debug.Log("debaff ON");        
        _speedIsDebaffed = true;
        ChangeCharacterSpeed();
    }

    public void BrakeOff()
    {
        Debug.Log("debaff OFF");
        _speedIsDebaffed = false; 
        ChangeCharacterSpeed();     
    }

    private void ChangeCharacterSpeed()
    {
        _character.SpeedMultiplier = _defaultCharacterSpeedMultiplier * (_speedIsDebaffed ? _model.DebafferSpeedMultiplier 
            : (_isMovingWithBoost ? _model.BoostSpeedMultiplier 
            : (_kaufmoIsActivated ? _model.KaufmoSpeedMultiplier 
            : 1f)));
    }

    private IEnumerator DebafferSpawnerReloading()
    {        
        _debafferSpawnerIsReady = false;
        foreach (var item in _onDebaffChanged) item.Invoke(_debafferSpawnerIsReady, _debaffersCount);
        yield return new WaitForSeconds(_model.DebafferSpawnCooldawn);
        _debafferSpawnerIsReady = true;
        foreach (var item in _onDebaffChanged) item.Invoke(_debafferSpawnerIsReady, _debaffersCount);
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



