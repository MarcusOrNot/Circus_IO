using UnityEngine;


public class Character : MonoBehaviour
{
    public float SpeedMultiplier { get { return _speedMultiplier; } set { _speedMultiplier = Mathf.Max(0, value); } }
    

    public void Move(Vector2 direction)
    {
        
        Quaternion targetRotation = transform.rotation * Quaternion.AngleAxis(Vector3.SignedAngle(transform.forward, new Vector3 (direction.x, 0, direction.y),Vector3.up), Vector3.up);  
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);        
        if (transform.rotation == targetRotation)
        {            
            _controller.Move(transform.forward * _model.Speed * _speedMultiplier * Time.deltaTime + Vector3.down); 
        }    
    }





    [SerializeField] private CharacterModel _model;
    private CharacterController _controller;
    private float _rotationSpeed = 100f;
    private float _speedMultiplier = 1.0f;
            

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    
}
