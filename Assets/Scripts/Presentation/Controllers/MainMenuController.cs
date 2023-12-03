using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Inject] private IGameStats _gameStats;
    [SerializeField] private InputField _nameField;
    [SerializeField] private GameObject _buttonStart;
    [SerializeField] private TextMeshProUGUI _connectingText;
    private void Start()
    {
        
        _nameField.text = _gameStats.PlayerName;
    }
    public void StartGame()
    {
        _gameStats.PlayerName = _nameField.text;
        SceneManager.LoadScene("GameScene");
        //StartCoroutine(ConnectorCoro(Random.Range(1,3)));
        //StartCoroutine(ConnectorCoro(5));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator ConnectorCoro(int delay)
    {
        _buttonStart.SetActive(false);
        _connectingText.gameObject.SetActive(true);
        var counter = 0.0f;
        var dots = 3;
        Debug.Log("Now coro is " + counter.ToString() + ", delay is " + delay.ToString());
        while (counter<delay)
        {
            Debug.Log("Now coro is " + counter.ToString() + ", delay is " + delay.ToString());
            var resText = "Connecting";
            for (int i = 0; i < dots; i++)
                resText += ".";
            _connectingText.text = resText;
            yield return new WaitForSeconds(0.5f);
            counter+=0.5f;
            dots++; if (dots > 3) dots = 0;
        }
        SceneManager.LoadScene("GameScene");
    }

    /*private void OnDestroy()
    {
        StopAllCoroutines();
    }*/
}
