using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Character))]

public class Hunter : MonoBehaviour
{
    public bool IsReadyToBoost { get { return _isReadyToBoost; } }

    public void Boost()
    {
        StartCoroutine(BoostMoving());
    }

    public void Move(Vector2 direction)
    {
        if (!_isBoosting) _character.Move(direction);
    }    







    [SerializeField] private HunterModel _model;    
    private Character _character;

    private bool _isReadyToBoost = true;
    private bool _isBoosting = false;    

    private int _health;

    [SerializeField] private Renderer[] _valuableColoredComponents;


    private void Awake()
    {
        _character = GetComponent<Character>();        
    }

    private void Start()
    {        
        SetColorOnColoredComponents(_model.Color);
        _health = _model.StartEntity;
        SetScale();        
        StartCoroutine(StandOnFloor());

        //StartCoroutine(SetUnvulnerablity());
    }

    private void Update()
    {
        if (_isBoosting) _character.Move(new Vector2(transform.forward.x, transform.forward.z));
    }


    private void SetColorOnColoredComponents(Color color)
    {
        foreach (Renderer valuableColoredComponent in _valuableColoredComponents)
            valuableColoredComponent.material.color = color;
    }
    
    private void SetScale()
    {
        transform.localScale = Vector3.one * (1 + (float)_health / 10);        
    }

    /*
      
    [SerializeField] private List<Entity> _entitiesPrefabs = new List<Entity>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Entity entity))
            EatEntity(entity);

    }

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

    private void GenerateEntities(int count)
    {
        float entityPullOutForce = 0.05f;
        for (int i = 1; i <= count; i++)
        {
            Instantiate(_entitiesPrefabs[Random.Range(0, _entitiesPrefabs.Count)], transform.position, Quaternion.identity)
                .GetComponent<Rigidbody>().AddForce((Vector3.up + Quaternion.AngleAxis(Random.Range(0, 359f), Vector3.up) * Vector3.forward) * entityPullOutForce, ForceMode.Impulse);
        }
    }

    private void EatEntity(Entity entity)
    {
        Destroy(entity.gameObject);
        _health++;
        SetScale();
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

    private IEnumerator BoostMoving()
    {
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
        _isReadyToBoost = true;
    }

    private IEnumerator StandOnFloor()
    {
        yield return new WaitForSeconds(0.3f);
        _character.Move(Vector2.up);
    }
}
