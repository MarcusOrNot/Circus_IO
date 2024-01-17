using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class LoaderSceneController : MonoBehaviour
{
    [Inject] private IGameStats _gameStats;
    [Inject] private LevelProcessService _levelProcessor;
    [SerializeField] private TextMeshProUGUI _connectingText;
    // Start is called before the first frame update
    void Start()
    {
        RuntimeInfo.LevelParams = _levelProcessor.GenerateLevel(_gameStats.GetStat(GameStatsType.KOEF_DIFFICULTY));
        Debug.Log("Runtime data info "+RuntimeInfo.LevelParams.MaxZoneSize.ToString());
        StartCoroutine(ConnectorCoro(Random.Range(2, 5)));
    }

    private IEnumerator ConnectorCoro(int delay)
    {
        _connectingText.gameObject.SetActive(true);
        var counter = 0.0f;
        var dots = 3;
        while (counter < delay)
        {
            var resText = "Connecting";
            for (int i = 0; i < dots; i++)
                resText += ".";
            _connectingText.text = resText;

            counter += 0.5f;
            dots++; if (dots > 3) dots = 0;

            yield return new WaitForSeconds(0.5f);
        }
        Utils.OpenScene(SceneType.GAME_ROYAL_BATTLE);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
