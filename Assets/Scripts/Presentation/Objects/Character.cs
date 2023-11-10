using System.Collections;
using UnityEngine;


public class Character : MonoBehaviour
{
    public float SpeedMultiplier { get { return _speedMultiplier; } set { _speedMultiplier = Mathf.Max(0, value); } }
       

    public void Move(Vector2 direction)
    {
        if ((direction.magnitude < _model.SensivityTreshold) || (Mathf.Abs(_rigidbody.velocity.y) > 1f)) 
        { 
            _rigidbody.velocity = new Vector3(0 , _rigidbody.velocity.y, 0);
            _isMoving = false;
            return; 
        }        
        Quaternion targetRotation = transform.rotation * Quaternion.AngleAxis(Vector3.SignedAngle(transform.forward, new Vector3(direction.x, 0, direction.y), Vector3.up), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _model.RotationSpeed * Time.deltaTime);
        if (transform.rotation == targetRotation)
        {            
            _isMoving = true;
            if (!_isStoppingProcessStart) StartCoroutine(StopMoving()); 
            direction *= _model.Speed * _speedMultiplier;
            _rigidbody.velocity = new Vector3(direction.x, _rigidbody.velocity.y, direction.y);    
        }       
    }





    [SerializeField] private CharacterModel _model;
    private Rigidbody _rigidbody;
    
    private float _speedMultiplier = 1.0f;
    private bool _isMoving = false;
    private bool _isStoppingProcessStart = false;

    private void Awake()
    {        
        _rigidbody = GetComponent<Rigidbody>();
    }  

    private IEnumerator StopMoving()
    {
        _isStoppingProcessStart = true;
        while (_isMoving)
        {
            _isMoving = false;
            yield return new WaitForSeconds(Mathf.Max(Time.deltaTime, Time.fixedDeltaTime) * 2);                       
        }
        Move(Vector2.zero);
        _isStoppingProcessStart = false;
    }    

}
