using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonKey : MonoBehaviour
{
    [SerializeField] private KeyCode _keyCode = KeyCode.None; 
    [SerializeField] private UnityEvent _onKey;
    private Image _keyImage;
    private void Awake()
    {
        _keyImage = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (_onKey != null && _keyCode != KeyCode.None)
        {
            if (Input.GetKey(_keyCode))
            {
                _onKey?.Invoke();
                _keyImage.color = Color.white;
            }
            else
            {
                _keyImage.color = new Color(1,1,1,0.5f);
            }
        }
    }

    public void SetOnKeyPressed(UnityAction onKey)
    {
        _onKey.AddListener(onKey);
    }

    private void OnDestroy()
    {
        _onKey.RemoveAllListeners();
    }
}
