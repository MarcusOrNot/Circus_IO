using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private IGameStats _stats;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Score is "+_stats.GetStat(GameStatsType.SCORE).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
