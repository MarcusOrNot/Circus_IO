using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonKey : MonoBehaviour
{
    [SerializeField] private KeyCode[] _keyCodes = new KeyCode[] { }; 
    [SerializeField] private UnityEvent _onKey;
    private Image _keyImage;
    private void Awake()
    {
        _keyImage = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (_onKey != null && _keyCodes.Length>0)
        {
            if (IsPressedKey())
            {
                _onKey?.Invoke();
                _keyImage.color = Color.white;
            }
            else
            {
                _keyImage.color = new Color(1, 1, 1, 0.5f);
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

    private bool IsPressedKey()
    {
        foreach (KeyCode key in _keyCodes)
            if (Input.GetKey(key))
                return true;
        return false;
    }
}
