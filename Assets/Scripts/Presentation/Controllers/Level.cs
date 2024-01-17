using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour, ILevelInfo
{
    [SerializeField] private Canvas _canvasHUD;
    private PlayerHunter _player;
    private DamageZoneConroller _damageZoneConroller;
    private EntityGenerator _entityGenerator;
    public static Level Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        _damageZoneConroller = FindObjectOfType<DamageZoneConroller>();
        _entityGenerator = FindObjectOfType<EntityGenerator>();
    }

    public IPlayer GetPlayer()
    {
        if (_player != null) return _player;
        
        _player = FindObjectOfType<PlayerHunter>();
        return _player;
    }

    public DamageZoneConroller GetDamageZone() => _damageZoneConroller;
    public EntityGenerator GetEntityGeneratoru => _entityGenerator;
    public Canvas GetHUDCanvas => _canvasHUD;

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public LevelParamsModel GetLevelParams()
    {
        return RuntimeInfo.LevelParams;
    }
}
