using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoPrefs : IGameInfo
{
    private const string FEED_VALUE = "Gameinfo_feed_value";

    public int FeedValue { get => PlayerPrefs.GetInt(FEED_VALUE, 0); set => PlayerPrefs.SetInt(FEED_VALUE, value); }
}
