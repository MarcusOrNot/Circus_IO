using System.Collections;
using System;
using UnityEngine;
using Zenject;


[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour
{
    [Inject] private EntityFactory _factory;    

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

    private Color _color;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    private void Start()
    {        
        _entityTypeForSpawn = (EntityType)Enum.GetValues(typeof(EntityType)).GetValue(0);
        SetColor();
        SetColorOnColoredComponents(_color);
        ChangeHealth(_model.StartEntity);         
        StartCoroutine(StandOnFloor());
        SetBoostReady();        
    }

    private void SetColor()
    {  
        _color = _model.HunterColor switch
        {
            HunterColor.BLACK => Color.black,
            HunterColor.BROWN => new Color(0.3f, 0.17f, 0.17f),
            HunterColor.DARK_GRAY => new Color(0.39f, 0.39f, 0.39f),
            HunterColor.BLUE => Color.blue,
            HunterColor.DARK_YELLOW => new Color(0.52f, 0.51f, 0f),
            HunterColor.DARK_RED => new Color(0.53f, 0f, 0f),
            HunterColor.DARK_GREEN => new Color(0f, 0.53f, 0f),
            HunterColor.DARK_ORANGE => new Color(0.7f, 0.4f, 0f),
            HunterColor.VIOLET => new Color(0.5f, 0f, 1f),
            HunterColor.DARK_PINK => new Color(0.57f, 0f, 0.54f),
            HunterColor.DARK_SKY => new Color(0f, 0.49f, 0.52f),
            _ => Color.white
        };        
        _color -= Color.white * _model.DarkColorMultiplier;
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

    private IEnumerator StandOnFloor()
    {
        yield return new WaitForSeconds(0.3f);
        _character.Move(Vector2.up);
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

    private void ChangeHealth(int value)
    {
        _health += value;
        _onHealthChanged?.Invoke(_health);
        SetBoostReady();
        if (_model.IsScaleDependFromHealth) transform.localScale = Vector3.one * (1 + (float)_health / 30);        
    }

    private void GetDamage(int value)
    {
        ChangeHealth(-value);        
        SpawnOutEntities(Mathf.Min(value, _health + value));
        if (_health <= 0) Destroy(gameObject);        
    }

    private void SpawnOutEntities(int summaryHealth)
    {
        do { summaryHealth -= SpawnOutEntity().HealthCount; } while (summaryHealth > 0);
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
        spawnedEntity.Color = _color;
        return spawnedEntity;
    }


    // Array entityTypes = Enum.GetValues(typeof(EntityType));
    // EntityType randomEntityType = (EntityType)entityTypes.GetValue(UnityEngine.Random.Range(0, entityTypes.Length));

    /*     
    SetOnHealthChanged(NowHealthChanged);

    private void NowHealthChanged(int health)
    {
    }
    */
    /*
    StartCoroutine(SetUnvulnerablity());

   [SerializeField] private List<Entity> _entitiesPrefabs = new List<Entity>();
    */

    /*
   private void GetDamage()
   {
       if (_isUnvulnerable) return;
       int health = _health;
       _health /= 2;
       StartCoroutine(SetUnvulnerablityTimer());
       GenerateEntities(health - _health);
       SetScale();
       if (_health <= 0) Destroy(gameObject);
   }
    */



    /*
    private IEnumerator SetUnvulnerablity()
    {
        _isUnvulnerable = true;
        Physics.IgnoreLayerCollision(8, 9, true);
        StartCoroutine(ColorFlashing());
        yield return new WaitForSeconds(_unvulnerablityTimer);
        Physics.IgnoreLayerCollision(8, 9, false);
        _isUnvulnerable = false;
    }
    private IEnumerator ColorFlashing()
    {
        while (_isUnvulnerable)
        {
            SetColorOnColoredComponents(Color.white);
            yield return new WaitForSeconds(0.2f);
            SetColorOnColoredComponents(_model.Color);
            yield return new WaitForSeconds(0.2f);
        }
    }
    */
}
