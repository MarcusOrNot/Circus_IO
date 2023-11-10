using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
