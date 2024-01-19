using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHunterSpawn : HunterSpawn
{
    [SerializeField] protected int _startLifes = 0;
    [Inject] private ISettings _settings;
    override public void Start()
    {
        var hunter = _factory.SpawnPlayerHunter(_hunter);
        hunter.SetHealth(_startLifes);
        hunter.SetHat(_settings.ChosenHat);
        SetCurrentPosition(hunter);
        Destroy(gameObject);
    }
}
