using System.Collections;
using UnityEngine;


public class Character : MonoBehaviour
{
    public float SpeedMultiplier { get { return _speedMultiplier; } set { _speedMultiplier = Mathf.Max(0, value); } }
       

    public void GetMovingCommand(Vector2 direction)
    {
        float fallingTrigger = -5f;
        if (_rigidbody == null) return;
        _isMoveCommandGet = !((direction.magnitude < _model.SensivityTreshold) || (_rigidbody.velocity.y < fallingTrigger));
        if (_isMoveCommandGet)
        {
            _movingDirection = direction;
            if (!_isMoving) { SetMovingStatus(true); StartCoroutine(MoveStoppingProcess()); }
        }        
    }


    [SerializeField] private CharacterModel _model;
    private Rigidbody _rigidbody;
    
    private float _speedMultiplier = 1.0f;
    private bool _isMoving = false;
    private bool _isBodyPositionFreezed = true;
    private bool _isMoveCommandGet = false;
    private float _stopMovingTimeMultiplier = 1f;

    private bool _isInCollision = false;    

    private Vector2 _movingDirection = Vector2.zero;

    private void Awake()
    {        
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.mass = 10;
        _rigidbody.drag = 0;        
    }

    private void SetMovingStatus(bool isMoving)
    {
        SetBodyPositionFreezeStatus(!isMoving);
        _isMoving = isMoving;
    }
    private void SetBodyPositionFreezeStatus(bool isFreeze)
    {
        if (_rigidbody == null) return;
        if (isFreeze == _isBodyPositionFreezed) { return; } else { _isBodyPositionFreezed = isFreeze; }
        if (isFreeze) { _rigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ; }
        else { _rigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ); }
    } 

    private void Move(Vector2 direction)
    {
        if (_rigidbody == null) return;
        if (!TryRotateToDirection(direction))
        {
            SetBodyPositionFreezeStatus(false);
            direction = direction.normalized * _model.Speed * _speedMultiplier;
            _rigidbody.velocity = new Vector3(direction.x, _rigidbody.velocity.y, direction.y);
        }
        else { SetBodyPositionFreezeStatus(true); }
    }    
    private bool TryRotateToDirection(Vector2 direction)
    {
        Quaternion targetRotation = transform.rotation * Quaternion.AngleAxis(Vector3.SignedAngle(transform.forward, new Vector3(direction.x, 0, direction.y), Vector3.up), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _model.RotationSpeed * Time.fixedDeltaTime);        
        return transform.rotation != targetRotation;
    }

    private IEnumerator MoveStoppingProcess()
    {          
        while (_isMoveCommandGet)
        {
            _isMoveCommandGet = false;
            yield return new WaitForSeconds(Mathf.Max(Time.deltaTime, Time.fixedDeltaTime) * _stopMovingTimeMultiplier);                       
        }
        SetMovingStatus(false);        
    }    
        

    private void OnCollisionEnter()
    {      
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
        StartCoroutine(CollisionExitTimer());
    }     
    private void OnCollisionExit()
    {
        _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }
    private IEnumerator CollisionExitTimer()
    {
        if (_isInCollision) { yield break; }
        _isInCollision = true;
        yield return new WaitForSeconds(0.1f);
        OnCollisionExit();
        _isInCollision = false;
    }

    private void FixedUpdate()
    {
        if (_isMoving) { Move(_movingDirection); }
    }
}
