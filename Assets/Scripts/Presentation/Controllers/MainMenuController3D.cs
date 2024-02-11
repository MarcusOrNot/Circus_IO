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
    [Inject] private IVibration _vibration;
    [Inject] private IData _data;
    [Inject] private ISystemInfo _systemInfo;
    //[Inject] private IAudioEffect _effect;
    [SerializeField] private MainMenuPanelController _mainMenu;
    [SerializeField] private ShopHatController _shopHatMenu;
    [SerializeField] private StartGameUI _startGameUI;
    [SerializeField] private RateUsUI _rateUS;
    [SerializeField] private GameObject _exitBTN;
    //[SerializeField] private TextMeshProUGUI _coinsCountValue;
    private Camera _camera;
    //private Transform _cameraStartTransform;
    private Vector3 _cameraStartPosition;
    //private Transform _mainTransformDelta;
    //private Transform _shopHatTransformDelta;
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        _camera = Camera.main;
        _cameraStartPosition = _camera.transform.position;
        if (_systemInfo.GetPlatformType()!=PlatformType.ANDROID)
            _exitBTN.SetActive(false);
    }
    private void Start()
    {
        _rateUS.ShowByCondtition2();
    }
    /*public void StartGame()
    {
        //if (_data.FeedValue==0 && RuntimeInfo.IsShownRate == false && RuntimeInfo.IsGamePlayedOnce == true)
        //{
        _rateUS.gameObject.SetActive(true);
        //}
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
        //_vibration.PlayMillis(2000);
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
