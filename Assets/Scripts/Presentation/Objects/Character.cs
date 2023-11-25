using System.Collections;
using UnityEngine;


public class Character : MonoBehaviour
{
    public float SpeedMultiplier 
    { 
        get { return _speedMultiplier; } 
        set { _speedMultiplier = Mathf.Max(0, value); SetGoingAnimationSpeed(); } 
    }       

    public void GetMovingCommand(Vector2 direction)
    {
        if (_rigidbody == null) return;
        float fallingTrigger = -5f;        
        _isMoveCommandGet = !((direction.magnitude < _model.SensivityTreshold) || (_rigidbody.velocity.y < fallingTrigger));
        if (_isMoveCommandGet)
        {
            _movingDirection = direction;
            if (!_isMoving) { SetMovingStatus(true); StartCoroutine(MoveStoppingProcess()); }
        }        
    }


    [SerializeField] private CharacterModel _model;
    private Rigidbody _rigidbody;
    private Animator _animator;
    
    private float _speedMultiplier = 1.0f;
   
    private bool _isMoving = false;
    private bool _isRotatingToLeft = false;
    private bool _isRotatingToRight = false;
    private bool _isMovingForward = false;
    private bool _isBodyPositionFreezed = true;
    private bool _isMoveCommandGet = false;
    private float _stopMovingTimeMultiplier = 1f;

    private bool _isInCollision = false;    

    private Vector2 _movingDirection = Vector2.zero;

    
    private void Awake()
    {  
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {        
        SetGoingAnimationSpeed();
    }
    private void SetGoingAnimationSpeed()
    {        
        _animator?.SetFloat("Speed", _model.Speed * _speedMultiplier / 10 / Mathf.Pow(transform.localScale.x, 0.3f));
    }


    private void SetMovingStatus(bool isMoving)
    {
        SetBodyPositionFreezeStatus(!isMoving);
        _isMoving = isMoving;
        if (!_isMoving || _isMovingForward || _isRotatingToLeft || _isRotatingToRight)
        {      
            _isMovingForward = _isRotatingToLeft = _isRotatingToRight = false;            
            _animator?.SetBool("Go", false);
            _animator?.SetBool("TurnLeft", false);
            _animator?.SetBool("TurnRight", false);
        }        
    }
    private void SetBodyPositionFreezeStatus(bool isFreeze)
    {
        if (_rigidbody == null) return;

        if (isFreeze == _isBodyPositionFreezed) { return; } 
        else { _isBodyPositionFreezed = isFreeze; }

        if (isFreeze) { _rigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ; }
        else { _rigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ); }
    }


    private void FixedUpdate()
    {
        if (_isMoving) { Move(_movingDirection); }
    }
    private void Move(Vector2 direction)
    {
        if (_rigidbody == null) return;
        if (!TryRotateToDirection(direction))
        {
            if (!_isMovingForward) {   _isMovingForward = true; _animator?.SetBool("Go", true); }
            SetBodyPositionFreezeStatus(false);
            direction = direction.normalized * _model.Speed * _speedMultiplier;
            _rigidbody.velocity = new Vector3(direction.x, _rigidbody.velocity.y, direction.y);
        }
        else 
        {            
            if (_isMovingForward) { _isMovingForward = false; _animator?.SetBool("Go", false); }
            SetBodyPositionFreezeStatus(true); 
        }
    }    
    private bool TryRotateToDirection(Vector2 direction)
    {
        Quaternion targetRotation = transform.rotation * Quaternion.AngleAxis(Vector3.SignedAngle(transform.forward, new Vector3(direction.x, 0, direction.y), Vector3.up), Vector3.up);
        float previousRotationAngle = transform.rotation.eulerAngles.y;  
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _model.RotationSpeed * Time.fixedDeltaTime);
        if (transform.rotation != targetRotation)
        {
            if (transform.rotation.eulerAngles.y < previousRotationAngle)
            {
                if (_isRotatingToRight) { _isRotatingToRight = false; _animator?.SetBool("TurnRight", false); }
                if (!_isRotatingToLeft) { _isRotatingToLeft = true; _animator?.SetBool("TurnLeft", true); }
            }
            else if (transform.rotation.eulerAngles.y > previousRotationAngle)
            {
                if (_isRotatingToLeft) { _isRotatingToLeft = false; _animator?.SetBool("TurnLeft", false); }
                if (!_isRotatingToRight) { _isRotatingToRight = true; _animator?.SetBool("TurnRight", true); }
            }
        }        
        else
        {
            if (_isRotatingToLeft) { _isRotatingToLeft = false; _animator?.SetBool("TurnLeft", false); }
            if (_isRotatingToRight) { _isRotatingToRight = false; _animator?.SetBool("TurnRight", false); }
        }        
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
        if (_rigidbody == null) return;
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
        StartCoroutine(CollisionExitTimer());
    }     
    private void OnCollisionExit()
    {
        if (_rigidbody == null) return;
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

}
