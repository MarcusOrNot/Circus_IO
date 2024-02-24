using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class ItemsProgressService
{
    private const string HATS_KEY = "Hats";
    private IProgressItems _progress;
    private ICloudGameStats _gameStats;
    private List<HatType> _cachedHats = new List<HatType>();
    [Inject]
    public ItemsProgressService(IProgressItems itemsProgress, ICloudGameStats gameStats)
    {
        _gameStats = gameStats;
        _progress = itemsProgress;
        RefreshHats();
    }
    public void InitProgressCloud()
    {
        _gameStats.GetGameData(new List<string>() { HATS_KEY }, (success, resData) =>
        {
            if (success)
            {
                if (resData.ContainsKey(HATS_KEY))
                {
                    var resHats = new List<HatType>();
                    var hatsStrings = JsonConvert.DeserializeObject<List<string>>(resData[HATS_KEY]);
                    foreach (string hatString in hatsStrings)
                    {
                        resHats.Add(Utils.GetEnumByString<HatType>(hatString));
                    }
                    _progress.InitHats(resHats);
                }
            }
        });
    }


    public bool IsHatOpened(HatType hat)
    {
        return _cachedHats.Contains(hat);
    }
    public void OpenHat(HatType hat)
    {
        _progress.OpenHat(hat);
        RefreshHats();
        var resData = new List<string>();
        var currentHats = _progress.OpenedHats;
        foreach (HatType hatType in currentHats)
        {
            resData.Add(hatType.ToString());
        }
        _gameStats.SetGameData(new Dictionary<string, string>() { { HATS_KEY, JsonConvert.SerializeObject(resData) } }, (res) => { });
    }
    private void RefreshHats()
    {
        _cachedHats = _progress.OpenedHats;
    }
}
