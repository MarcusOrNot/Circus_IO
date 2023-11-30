using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayersInfoService
{
    private List<PlayerInfoModel> _playersInfo;
    private List<int> _currentPlayers = new List<int>();
    public PlayersInfoService(List<PlayerInfoModel> playersInfo)
    {
        _playersInfo = playersInfo;
    }
    public PlayerInfoModel GeneratePlayerInfo()
    {
        List<PlayerInfoModel> freeList = GetFreeList();
        int randomPos = Random.Range(0, freeList.Count);
        _currentPlayers.Add(randomPos);
        return freeList[randomPos];
    }
    private List<PlayerInfoModel> GetFreeList()
    {
        List<PlayerInfoModel> currentList = new List<PlayerInfoModel>(_playersInfo);
        foreach(int pos in _currentPlayers)
        {
            currentList.Remove(_playersInfo[pos]);
        }
        return currentList;
    }
    public void ClearList()
    {
        _currentPlayers.Clear();
    }
}
