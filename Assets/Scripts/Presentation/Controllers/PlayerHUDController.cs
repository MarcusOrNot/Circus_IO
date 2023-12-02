using AppodealStack.Monetization.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHUDController : MonoBehaviour
{
    private float OFFSET = -70.0f;
    [SerializeField] private Image _playerImage;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _lifesValueText;
    [Inject] private PlayersInfoService _playersInfoService;
    private Hunter _hunter;
    private RectTransform _canvasRect = null;
    private Camera _camera = null;
    private PlayerInfoModel _playerInfoModel;
    // Start is called before the first frame update
    void Start()
    {
        _hunter = GetComponentInParent<Hunter>();
        _canvasRect = Level.Instance.GetHUDCanvas?.GetComponent<RectTransform>();
        _camera = Camera.main;
        if (_hunter== null || _camera == null || _canvasRect == null) Destroy(gameObject);
        gameObject.transform.parent = _canvasRect;
        _playerInfoModel = _playersInfoService.GeneratePlayerInfo();
        _playerImage.sprite = _playerInfoModel.PlayerIcon;
        _playerName.text = _playerInfoModel.PlayerName;
        _hunter.SetOnHealthChanged((lifes) => { UpdateLifes(); });
        UpdateLifes();
    }

    // Update is called once per frame
    void Update()
    {
        if (_hunter==null)
        {
            Destroy(gameObject);
            return;
        }
        var viewPos = _camera.WorldToViewportPoint(_hunter.transform.position);
        //float cameraToHunterDistanceOffset = 18f / Vector3.Distance(_camera.transform.position, _hunter.transform.position);
        Vector2 finalPosition = new Vector2(viewPos.x * _canvasRect.sizeDelta.x, viewPos.y * _canvasRect.sizeDelta.y+OFFSET);
        GetComponent<RectTransform>().anchoredPosition = finalPosition;
    }

    private void UpdateLifes()
    {
        _lifesValueText.text = _hunter.Lifes.ToString();
    }
}
