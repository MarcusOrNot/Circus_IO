using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshPro))]

public class HunterHUD : MonoBehaviour
{
    private Camera _camera;
    private TextMeshPro _HUD;
    private Hunter _hunter;
    

    private void Awake()
    {        
        _camera = Camera.main;
        _HUD = GetComponent<TextMeshPro>();        
        _hunter = GetComponentInParent<Hunter>();        
        _hunter?.SetOnHealthChanged(health => ChangeText(health));   
    }
    private void Start()
    {
        if (_hunter == null) { Destroy(gameObject); return; }  
        transform.SetParent(_camera.transform, false);
    }
    private void Update()
    {
        if (_hunter == null) { Destroy(gameObject); return; }        
        transform.position = GetPositionAtCamera() + GetPositionCorrectionToShowUnderHunter();
        transform.rotation = _camera.transform.rotation;           
    }


    private Vector3 GetPositionAtCamera()
    {
        return Vector3.MoveTowards(_camera.transform.position, _hunter.transform.position, 4.5f);
    }

    private Vector3 GetPositionCorrectionToShowUnderHunter()
    {
        float downOffset = 0.7f;
        float hunterScaleOffset = _hunter.transform.lossyScale.x / 2f;
        float cameraToHunterDistanceOffset = 18f / Vector3.Distance(_camera.transform.position, _hunter.transform.position);
        return -_camera.transform.up * downOffset * hunterScaleOffset * cameraToHunterDistanceOffset;
    }

    
    private void ChangeText(int health)
    {        
        _HUD.text = health.ToString();
    }
}
