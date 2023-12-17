using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _buttonStart;
    [SerializeField] private TextMeshProUGUI _connectingText;

    public void StartRoyalGame()
    {
        StartCoroutine(ConnectorCoro(Random.Range(1, 3)));
    }

    private IEnumerator ConnectorCoro(int delay)
    {
        _buttonStart.SetActive(false);
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
