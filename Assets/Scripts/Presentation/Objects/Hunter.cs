using System.Collections;
using System;
using UnityEngine;


[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour
{
    public HunterModel Model { get { return _model; } }
    public bool IsReadyToBoost { get { return _isReadyToBoost; } }
    
    public void Boost() { StartCoroutine(BoostMoving()); }
    public void Move(Vector2 direction) { if (!_isBoosting) _character.Move(direction); }
    public void SetOnHealthChanged(Action<int> onHealthChanged) {  _onHealthChanged = onHealthChanged; }



    [SerializeField] private HunterModel _model;    
    private Character _character;

    private bool _isReadyToBoost = true;
    private bool _isBoosting = false;    

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
    private void Update()
    {
        if (_isBoosting) _character.Move(new Vector2(transform.forward.x, transform.forward.z));
    }

    private void SetColorOnColoredComponents(Color color)
    {
        foreach (Renderer coloredComponent in _model.ColoredComponents) coloredComponent.material.color = color;
    }    
    private void SetScale()
    {
        transform.localScale = Vector3.one * (1 + (float)_health / 10);        
    }   

    private IEnumerator BoostMoving()
    {
        if (!_isReadyToBoost) yield break;
        StartCoroutine(BoosterCooldown());
        _isBoosting = true;
        float savedBoostSpeed = _character.SpeedMultiplier;        
        _character.SpeedMultiplier = _model.BoostValue;       
        yield return new WaitForSeconds(_model.BoostTime);
        _character.SpeedMultiplier = savedBoostSpeed;        
        _isBoosting = false;
    }
    private IEnumerator BoosterCooldown()
    {
        _isReadyToBoost = false;
        yield return new WaitForSeconds(_model.BoostRestartTime);
        SetBoostReady();
    }

    private IEnumerator StandOnFloor()
    {
        yield return new WaitForSeconds(0.3f);
        _character.Move(Vector2.up);
    }

    private void SetBoostReady() { _isReadyToBoost = _health > _model.BoostPrice; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity)) EatEntity(entity);   
    }

    private void EatEntity(Entity entity)
    {
        Destroy(entity.gameObject);
        ChangeHealth(1);        
    }

    private void ChangeHealth(int value)
    {
        _health += value;
        SetScale();
        _onHealthChanged?.Invoke(_health);
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
   private void GenerateEntities(int count)
   {
       float entityPullOutForce = 0.05f;
       for (int i = 1; i <= count; i++)
       {
           Instantiate(_entitiesPrefabs[Random.Range(0, _entitiesPrefabs.Count)], transform.position, Quaternion.identity)
               .GetComponent<Rigidbody>().AddForce((Vector3.up + Quaternion.AngleAxis(Random.Range(0, 359f), Vector3.up) * Vector3.forward) * entityPullOutForce, ForceMode.Impulse);
       }
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
