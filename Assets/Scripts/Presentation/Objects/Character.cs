using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class Character : MonoBehaviour
{
    public float SpeedMultiplier 
    { 
        get { return _speedMultiplier; } 
        set { _speedMultiplier = Mathf.Max(1f, value); SetGoingAnimationSpeed(); } 
    }    

    public void GetMovingCommand(Vector2 direction)
    {
        const float DIRECTION_MAGNITUDE_ZERO_TRIGGER = 0.3f, RIGIDBODY_FALLING_TRIGGER = -5f;
        if (_rigidbody == null) return; 
        _isMoveCommandGet = !((direction.magnitude < DIRECTION_MAGNITUDE_ZERO_TRIGGER) || (_rigidbody.velocity.y < RIGIDBODY_FALLING_TRIGGER));
        if (_isMoveCommandGet)
        {
            _movingDirection = direction;
            if (!_isMoving) { SetMovingStatus(true); StartCoroutine(MoveStoppingProcess()); }
        }        
    }
    /*
    public void ChangeAnimator() 
    {
        _animator = GetComponentInChildren<Animator>();        
        _isMovingForward = false;
    }
    */
    public void StartAttackAnimation()
    {
        //_animator.SetTrigger("Attack");
        foreach (Animator animator in _animators) animator.SetTrigger("Attack");
    }


    [SerializeField] private CharacterModel _model;
    private Rigidbody _rigidbody;
    private Animator[] _animators;
    
    private float _speedMultiplier = 1.0f;    
   
    private bool _isMoving = false;
    private bool _isRotatingToLeft = false;
    private bool _isRotatingToRight = false;
    private bool _isMovingForward = false;
    private bool _isBodyPositionFreezed = true;
    private bool _isMoveCommandGet = false;
    private float _stopMovingTimeMultiplier = 1f;

    //private bool _isInCollision = false;    

    private Vector2 _movingDirection = Vector2.zero;

    private Hunter _currentCollidedHunter = null;

    
    private void Awake()
    {  
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        _animators = GetComponentsInChildren<Animator>();        
    }

    private void Start()
    {        
        SetGoingAnimationSpeed();
    }
    private void SetGoingAnimationSpeed()
    {
        foreach (Animator animator in _animators) { animator?.SetFloat("Speed", _model.Speed * _speedMultiplier / 10 / Mathf.Pow(transform.localScale.x, 0.3f)); }
        //_animator?.SetFloat("Speed", _model.Speed * _speedMultiplier / 10 / Mathf.Pow(transform.localScale.x, 0.3f));
    }


    private void SetMovingStatus(bool isMoving)
    {
        SetBodyPositionFreezeStatus(!isMoving);
        _isMoving = isMoving;
        if (!_isMoving || _isMovingForward || _isRotatingToLeft || _isRotatingToRight)
        {      
            _isMovingForward = _isRotatingToLeft = _isRotatingToRight = false;
            foreach (Animator animator in _animators)
            {
                animator?.SetBool("Go", false);
                animator?.SetBool("TurnLeft", false);
                animator?.SetBool("TurnRight", false);
            }
            //_animator?.SetBool("Go", false);
            //_animator?.SetBool("TurnLeft", false);
            //_animator?.SetBool("TurnRight", false);
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
            //if (!_isMovingForward) {   _isMovingForward = true; _animator?.SetBool("Go", true); }
            if (!_isMovingForward) { _isMovingForward = true; foreach (Animator animator in _animators) animator?.SetBool("Go", true); }
            SetBodyPositionFreezeStatus(false);
            direction = direction.normalized * _model.Speed * _speedMultiplier;
            _rigidbody.velocity = new Vector3(direction.x, _rigidbody.velocity.y, direction.y);
        }
        else 
        {
            //if (_isMovingForward) { _isMovingForward = false; _animator?.SetBool("Go", false); }
            if (_isMovingForward) { _isMovingForward = false; foreach (Animator animator in _animators) animator?.SetBool("Go", false); }
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
                //if (_isRotatingToRight) { _isRotatingToRight = false; _animator?.SetBool("TurnRight", false); }
                if (_isRotatingToRight) { _isRotatingToRight = false; foreach (Animator animator in _animators) animator?.SetBool("TurnRight", false); }
                //if (!_isRotatingToLeft) { _isRotatingToLeft = true; _animator?.SetBool("TurnLeft", true); }
                if (!_isRotatingToLeft) { _isRotatingToLeft = true; foreach (Animator animator in _animators) animator?.SetBool("TurnLeft", true); }
            }
            else if (transform.rotation.eulerAngles.y > previousRotationAngle)
            {
                //if (_isRotatingToLeft) { _isRotatingToLeft = false; _animator?.SetBool("TurnLeft", false); }
                if (_isRotatingToLeft) { _isRotatingToLeft = false; foreach (Animator animator in _animators) animator?.SetBool("TurnLeft", false); }
                //if (!_isRotatingToRight) { _isRotatingToRight = true; _animator?.SetBool("TurnRight", true); }
                if (!_isRotatingToRight) { _isRotatingToRight = true; foreach (Animator animator in _animators) animator?.SetBool("TurnRight", true); }
            }
        }        
        else
        {
            //if (_isRotatingToLeft) { _isRotatingToLeft = false; _animator?.SetBool("TurnLeft", false); }
            if (_isRotatingToLeft) { _isRotatingToLeft = false; foreach (Animator animator in _animators) animator?.SetBool("TurnLeft", false); }
            //if (_isRotatingToRight) { _isRotatingToRight = false; _animator?.SetBool("TurnRight", false); }
            if (_isRotatingToRight) { _isRotatingToRight = false; foreach (Animator animator in _animators) animator?.SetBool("TurnRight", false); }
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
        

    private void OnCollisionEnter(Collision collision)
    {
        if (_rigidbody == null) return;
        if (collision.gameObject.TryGetComponent(out Hunter hunter))
        {
            _currentCollidedHunter = hunter;
            StartCoroutine(VerticalMoveFreezing(hunter));
            
            
        }
        
        //StartCoroutine(CollisionExitTimer());
    }    
    private IEnumerator VerticalMoveFreezing(Hunter hunter)
    {
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
        while ((hunter != null) && (_currentCollidedHunter == hunter))
        {            
            yield return new WaitForSeconds(0.1f);
        }
        if (hunter == null) { _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY; _currentCollidedHunter = null; }
        
    }    
    private void OnCollisionExit(Collision collision)
    {
        if (_rigidbody == null) return;
        if (collision.gameObject.TryGetComponent(out Hunter hunter) && (hunter == _currentCollidedHunter))
        {
            _currentCollidedHunter = null;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;            
        }
    }
    
    /*
    private IEnumerator CollisionExitTimer()
    {
        if (_isInCollision) { yield break; }
        _isInCollision = true;
        yield return new WaitForSeconds(0.1f);
        OnCollisionExit();
        _isInCollision = false;
    } 
    */
        
}
