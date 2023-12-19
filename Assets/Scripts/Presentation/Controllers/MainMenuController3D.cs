using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using System;

public class MainMenuController3D : MonoBehaviour
{
    [Inject] private IGameStats _gameStats;
    [SerializeField] private MainMenuPanelController _mainMenu;
    [SerializeField] private ShopHatController _shopHatMenu;
    [SerializeField] private StartGameUI _startGameUI;
    [SerializeField] private TextMeshProUGUI _coinsCountValue;
    private Camera _camera;
    //private Transform _cameraStartTransform;
    private Vector3 _cameraStartPosition;
    //private Transform _mainTransformDelta;
    //private Transform _shopHatTransformDelta;
    private void Awake()
    {
        _camera = Camera.main;
        _cameraStartPosition = _camera.transform.position;
    }
    private void Start()
    {
        _coinsCountValue.text = _gameStats.GetStat(GameStatsType.COINS).ToString();
    }
    /*public void StartGame()
    {
        _gameStats.PlayerName = _nameField.text;
        _startGameUI.StartRoyalGame();
        //SceneManager.LoadScene("GameScene");
        //StartCoroutine(ConnectorCoro(5));
    }*/
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ShowMainMenu()
    {
        //HideMenus();
        ShowMenuAnim(new Vector3(0, 0, 0), () => _mainMenu.gameObject.SetActive(true));
        //_mainMenu.gameObject.SetActive(true);
    }
    public void ShowHatsShopMenu()
    {
        //HideMenus();
        ShowMenuAnim(new Vector3(-2.5f,0,0), ()=> _shopHatMenu.gameObject.SetActive(true));
        //_shopHatMenu.gameObject.SetActive(true);
    }
    private void HideMenus()
    {
        _mainMenu.gameObject.SetActive(false);
        _shopHatMenu.gameObject.SetActive(false);
    }
    private void ShowMenuAnim(Vector3 deltaPos, Action onEnd)
    {
        HideMenus();
        var sequence = DOTween.Sequence();
        sequence.Append(_camera.transform.DOLocalMove(_cameraStartPosition+ deltaPos, 1.0f)).OnComplete(()=>onEnd?.Invoke());
    }
}
