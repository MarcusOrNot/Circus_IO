using System.Collections;
using System;
using UnityEngine;
using Zenject;


[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour
{
    [Inject] private EntityFactory _factory;    

    public HunterModel Model { get { return _model; } }
    public bool IsReadyToBoost { get { return _isBoosterReady; } }
    
    public void Boost() { StartCoroutine(BoosterActivation()); }
    public void Move(Vector2 direction) { if (!_isMovingWithBoost) _character.Move(direction); }

    public void SetOnHealthChanged(Action<int> onHealthChanged) {  _onHealthChanged = onHealthChanged; }



    [SerializeField] private HunterModel _model;    
    private Character _character;

    private bool _isBoosterReady = true;
    private bool _isMovingWithBoost = false;    

    private int _health = 0;
    private Action<int> _onHealthChanged;        

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    private void Start()
    {        
        SetColorOnColoredComponents(_model.Color);
        ChangeHealth(_model.StartEntity);         
        StartCoroutine(StandOnFloor());
        SetBoostReady();        
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
        yield return new WaitForSeconds(_model.BoostRestartTime);
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

    private void SetBoostReady() { _isBoosterReady = _health > _model.BoostPrice; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            Destroy(entity.gameObject);
            ChangeHealth(1);
        }
    }   

    private void ChangeHealth(int value)
    {
        _health += value;
        if (_model.IsScaleDependFromHealth) transform.localScale = Vector3.one * (1 + (float)_health / 10);
        _onHealthChanged?.Invoke(_health);
    }

    private void GetDamage(int value)
    {
        ChangeHealth(-value);
        SpawnOutEntities(Mathf.Min(value, _health + value));
        if (_health <= 0) Destroy(gameObject);        
    }

    private void SpawnOutEntities(int count)
    {
        Array entityTypes = Enum.GetValues(typeof(EntityType)); 
        float pullOutForceUp = 5f;
        float pullOutForceHorizontal = 3f;
        float positionVerticalOffset = transform.localScale.x * 1f;
        float positionHorisontalOffset = transform.localScale.x * 1f;        
        for (int i = 1; i <= count; i++)
        {
            EntityType randomEntityType = (EntityType)entityTypes.GetValue(UnityEngine.Random.Range(0, 2));
            float angle = UnityEngine.Random.Range(45, 315f);
            Vector3 horizontalDirection = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
            Entity spawnedEntity = _factory.Spawn(randomEntityType);
            spawnedEntity.transform.position = transform.position + horizontalDirection * positionHorisontalOffset + Vector3.up * positionVerticalOffset;
            spawnedEntity.GetComponent<Rigidbody>().AddForce(Vector3.up * pullOutForceUp + horizontalDirection * pullOutForceHorizontal, ForceMode.Impulse);
        }
    }


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
