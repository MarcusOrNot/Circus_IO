using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HunterHUD : MonoBehaviour
{
    private Camera _camera;
    private TextMeshPro _HUD;
    private Hunter _hunter;
    private bool _hudIsReady;    
    [SerializeField] private bool _hudOnlyOnAI = true;
    

    private void Awake()
    {        
        _camera = Camera.main;
        _HUD = GetComponentInChildren<TextMeshPro>();        
        _hunter = GetComponentInParent<Hunter>();
        _hunter?.SetOnHealthChanged(health => ChangeText(health));   
    }
    private void Start()
    {
        _hudIsReady = !(_HUD == null || _camera == null || _hunter == null || (_hudOnlyOnAI && _hunter.TryGetComponent(out PlayerHunter _)));
        if (!_hudIsReady) { Destroy(gameObject); return; }
        
        transform.SetParent(_camera.transform, false);
    }

    private void Update()
    {
        if (!_hudIsReady || _hunter == null) { Destroy(gameObject); return; }

        SetHUDPositionAtCamera();
        transform.rotation = _camera.transform.rotation;
        SetHUDPositionUnderHunter();        
    }
    
    private void SetHUDPositionAtCamera()
    {
        Vector3 newPosition = Vector3.MoveTowards(_camera.transform.position, _hunter.transform.position, 4.5f);
        transform.position = newPosition; 
    }

    private void SetHUDPositionUnderHunter()
    {
        float downOffset = 0.7f;
        float hunterScaleOffset = _hunter.transform.lossyScale.x / 2f;
        float cameraToHunterDistanceOffset = 18f / Vector3.Distance(_camera.transform.position, _hunter.transform.position);        
        transform.position += -transform.up * downOffset * hunterScaleOffset * cameraToHunterDistanceOffset;
    }
        

    private void ChangeText(int health)
    {
        if (!_hudIsReady) { return; }
        _HUD.text = health.ToString();
    }

}
