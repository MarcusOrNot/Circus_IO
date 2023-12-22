using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHunterSpawn : HunterSpawn
{
    [Inject] private ISettings _settings;
    override public void Start()
    {
        var hunter = _factory.SpawnPlayerHunter(_hunter);
        hunter.SetHat(_settings.ChosenHat);
        SetPosition(hunter);
        Destroy(gameObject);
    }
}
