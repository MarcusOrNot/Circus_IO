using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HunterHUD : MonoBehaviour
{
    private Camera _camera;
    private TextMesh _HUD;
    private Hunter _hunter;
    private bool _hudIsReady;
    [SerializeField] private Vector3 _HUDScale = new Vector3(2f, 2f, 2f);
    [SerializeField] private bool _hudOnlyOnAI = true;
    

    private void Awake()
    {        
        _camera = Camera.main;
        _HUD = GetComponentInChildren<TextMesh>();
        _hunter = GetComponentInParent<Hunter>();
        _hunter?.SetOnHealthChanged(health => ChangeText(health));
        _hunter?.SetOnScaleChanged(scale => HoldHUDScale(_HUDScale));

    }
    private void Start()
    {
        _hudIsReady = _camera != null && _HUD != null && _hunter != null && (!_hudOnlyOnAI || !_hunter.TryGetComponent(out PlayerHunter _));
        if (!_hudIsReady && _HUD != null) _HUD.gameObject.SetActive(false); 
        

    }
    private void Update()
    {        
        if (_hudIsReady && _hunter != null)
        {
            _HUD.transform.rotation = _camera.transform.rotation;
            _HUD.transform.position = _hunter.transform.position + new Vector3(0, 1f + _hunter.transform.lossyScale.y * 0.3f, -2.2f - _hunter.transform.lossyScale.x * 0.5f);
            
            
        }
    }        
    
    private void HoldHUDScale(Vector3 scale)
    {
        if (!_hudIsReady) { return; }
        Vector3 scaleMultiplier = new Vector3(scale.x / _HUD.transform.lossyScale.x, scale.y / _HUD.transform.lossyScale.y, scale.z / _HUD.transform.lossyScale.z);
        _HUD.transform.localScale = Vector3.Scale(_HUD.transform.localScale, scaleMultiplier);
    }

    private void ChangeText(int health)
    {
        if (!_hudIsReady) { return; }
        _HUD.text = health.ToString();
    }

}
