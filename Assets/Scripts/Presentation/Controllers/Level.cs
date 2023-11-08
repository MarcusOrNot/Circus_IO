using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private PlayerHunter _player;
    public static Level Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public IPlayer GetPlayer()
    {
        if (_player != null) return _player;
        
        _player = FindObjectOfType<PlayerHunter>();
        return _player;
    }
}
