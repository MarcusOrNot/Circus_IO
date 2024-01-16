using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventType
{
    PLAYER_SPAWNED,
    PLAYER_DEAD,
    PLAYER_ATE_HUNTER,
    PLAYER_HEALTH_CHANGED,
    HUNTER_SPAWNED,
    HUNTER_DEAD,
    GAME_PAUSED,
    GAME_CONTINUE,
    GAME_AD_PAUSED,
    ZONE_CHANGED
}
