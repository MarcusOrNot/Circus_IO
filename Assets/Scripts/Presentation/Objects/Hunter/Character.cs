using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class Character : MonoBehaviour
{
    public float SpeedMultiplier  {  get => _speedMultiplier;   set { _speedMultiplier = Mathf.Max(0, value); SendMovingSpeedToHunterForms();} }   
    public HunterVisualForm MainForm { get => _mainForm; set => _mainForm = value; }
    public HunterVisualForm SecondForm { get => _secondForm; }

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
    
    public void StartAttackAnimation() {  SendMovingStateToHunterForms(HunterMovingState.ATTACK); }
       

    
    [SerializeField] private CharacterModel _model;
    private Rigidbody _rigidbody;
    
    private float _speedMultiplier = 1.0f;    
   
    private bool _isMoving = false;
    private bool _isRotatingToLeft = false;
    private bool _isRotatingToRight = false;
    private bool _isMovingForward = false;
    private bool _isBodyPositionFreezed = true;
    private bool _isMoveCommandGet = false;
    private float _stopMovingTimeMultiplier = 1f;
     
    private Vector2 _movingDirection = Vector2.zero;

    private Hunter _currentCollidedHunter = null;

    private HunterVisualForm _mainForm = null;
    private HunterVisualForm _secondForm = null;


    private void Awake()
    {  
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;                
        if (_model.IsPacman) 
        { 
            _mainForm = GetComponentInChildren<PacmanForm>(true);
            if (_mainForm != null) { _mainForm.gameObject.SetActive(true);  GetComponentInChildren<BubbleForm>()?.gameObject.SetActive(false); }
            else { _mainForm = GetComponentInChildren<BubbleForm>(true); _mainForm.gameObject.SetActive(true); }
        }
        else
        {
            _mainForm = GetComponentInChildren<BubbleForm>(true);
            if (_mainForm != null)  {  _mainForm.gameObject.SetActive(true); GetComponentInChildren<PacmanForm>()?.gameObject.SetActive(false); }
            else { _mainForm = GetComponentInChildren<PacmanForm>(true); _mainForm.gameObject.SetActive(true); }            
        }        
        _secondForm = GetComponentInChildren<KaufmoForm>(true); 
        _secondForm.gameObject.SetActive(true);
    }
    private void Start()
    {        
        SendMovingSpeedToHunterForms();        
    }
    private void FixedUpdate()
    {
        if (_isMoving) { Move(_movingDirection); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_rigidbody == null) return;
        if (collision.gameObject.TryGetComponent(out Hunter hunter))
        {
            _currentCollidedHunter = hunter;
            StartCoroutine(VerticalMoveFreezing(hunter));
        }
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


    private void SetMovingStatus(bool isMoving)
    {
        SetBodyPositionFreezeStatus(!isMoving);
        _isMoving = isMoving;
        if (!_isMoving || _isMovingForward || _isRotatingToLeft || _isRotatingToRight)
        {      
            _isMovingForward = _isRotatingToLeft = _isRotatingToRight = false;            
            AnalyzeAndSendMovingStateToHunterForms();
        }        
    }

    private void SetBodyPositionFreezeStatus(bool isFreeze)
    {
        if (_rigidbody == null) return;
        if (isFreeze == _isBodyPositionFreezed) return; else _isBodyPositionFreezed = isFreeze;
        if (isFreeze) 
            _rigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ; 
        else 
            _rigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ); 
    }
        
    private void Move(Vector2 direction)
    {
        if (_rigidbody == null) return;
        if (!TryRotateToDirection(direction))
        {
            if (!_isMovingForward) { _isMovingForward = true; AnalyzeAndSendMovingStateToHunterForms(); }    
            SetBodyPositionFreezeStatus(false);
            direction = direction.normalized * _model.Speed * _speedMultiplier;
            _rigidbody.velocity = new Vector3(direction.x, _rigidbody.velocity.y, direction.y);
        }
        else 
        {
            if (_isMovingForward) { _isMovingForward = false; AnalyzeAndSendMovingStateToHunterForms(); }            
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
                if (_isRotatingToRight || !_isRotatingToLeft)  { _isRotatingToRight = false; _isRotatingToLeft = true; AnalyzeAndSendMovingStateToHunterForms(); }                    
            }
            else if (transform.rotation.eulerAngles.y > previousRotationAngle)
            {
                if (!_isRotatingToRight || _isRotatingToLeft) { _isRotatingToRight = true; _isRotatingToLeft = false; AnalyzeAndSendMovingStateToHunterForms(); }                
            }
        }        
        else
        {
            if (_isRotatingToRight || _isRotatingToLeft) { _isRotatingToRight = false; _isRotatingToLeft = false; AnalyzeAndSendMovingStateToHunterForms(); }            
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
      
    private IEnumerator VerticalMoveFreezing(Hunter hunter)
    {
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
        while ((hunter != null) && (_currentCollidedHunter == hunter)) {  yield return new WaitForSeconds(0.1f);  }
        if (hunter == null) { _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY; _currentCollidedHunter = null; }        
    } 

    private void AnalyzeAndSendMovingStateToHunterForms()
    {
        if (!_isMovingForward && !_isRotatingToLeft && !_isRotatingToRight) { SendMovingStateToHunterForms(HunterMovingState.IDLE); return; }
        if (_isMovingForward) SendMovingStateToHunterForms(HunterMovingState.GO_FORWARD); else SendMovingStateToHunterForms(HunterMovingState.STOP);
        if (_isRotatingToLeft) 
            SendMovingStateToHunterForms(HunterMovingState.TURN_LEFT);
        else if (_isRotatingToRight) 
            SendMovingStateToHunterForms(HunterMovingState.TURN_RIGHT);
        else
            SendMovingStateToHunterForms(HunterMovingState.NO_TURN);
    }
    private void SendMovingStateToHunterForms(HunterMovingState movingState) 
    {   
        _mainForm?.SetAnimationState(movingState);
        _secondForm?.SetAnimationState(movingState);
    }    
    private void SendMovingSpeedToHunterForms() 
    {
        float speed = _model.Speed * _speedMultiplier / 10 / Mathf.Pow(transform.localScale.x, 0.3f);
        _mainForm?.SetAnimationSpeed(speed);
        _secondForm?.SetAnimationSpeed(speed);
    }





    /*
    private IEnumerator ActivateSecondForm(float time)
    {
        const float HUNTER_MODEL_FLICKING_TIME_TRIGGER = 5f;
        _kaufmoIsActivated = true; foreach (var item in _onKaufmoActivated) item?.Invoke(_kaufmoIsActivated);
        SetBoostReadyState();
        _boosterIsReloading = false;
        _isMovingWithBoost = false; foreach (var item in _onBoostingStateChanged) item?.Invoke(_isMovingWithBoost);
        _secondForm.SetVisiblityStatus(true);
        _mainForm.SetVisiblityStatus(false);
        _character.SpeedMultiplier = _model.KaufmoSpeedMultiplier;

        yield return new WaitForSeconds(time - HUNTER_MODEL_FLICKING_TIME_TRIGGER);

        StartCoroutine(HunterModelFlicking(HUNTER_MODEL_FLICKING_TIME_TRIGGER));

        yield return new WaitForSeconds(HUNTER_MODEL_FLICKING_TIME_TRIGGER);

        _character.SpeedMultiplier = _defaultCharacterSpeedMultiplier;
        _mainForm.SetVisiblityStatus(true);
        _secondForm.SetVisiblityStatus(false);
        _kaufmoIsActivated = false; foreach (var item in _onKaufmoActivated) item.Invoke(_kaufmoIsActivated);
        SetBoostReadyState();
    }
    private IEnumerator HunterModelFlicking(float flickingTime)
    {
        const float HUNTER_FLICKING_PERIOD = 0.3f;
        bool mainFormIsVisible = false;
        while ((flickingTime > 0) && _kaufmoIsActivated)
        {
            mainFormIsVisible = !mainFormIsVisible;
            _mainForm?.SetVisiblityStatus(mainFormIsVisible);
            _secondForm?.SetVisiblityStatus(!mainFormIsVisible);

            //_mainForm?.SetVisiblityStatus(!_mainForm.IsVisible);
            //_secondForm?.SetVisiblityStatus(!_secondForm.IsVisible);
            yield return new WaitForSeconds(HUNTER_FLICKING_PERIOD);
            flickingTime -= HUNTER_FLICKING_PERIOD;
        }
        _secondForm.SetVisiblityStatus(false);
        _mainForm.SetVisiblityStatus(true);
    }
    */







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

    /*
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
    */
}
