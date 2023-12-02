using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraPlayerFollow : MonoBehaviour
{
    //private IPlayer _player;
    [SerializeField] private Vector3 _startOffset = new Vector3(-0.6686f, 17.184f, -19.58f);
    [SerializeField] private float _speed = 4f;

    private void Start()
    {
        //_player = Level.Instance.GetPlayer();
        if (Level.Instance.GetPlayer()!=null)
        {
            GetComponentInChildren<AudioListener>().transform.position = Level.Instance.GetPlayer().GetPosition();
        }
    }

    private void Update()
    {
        //if (_player != null) transform.position = _player.GetPosition() + _startOffset;
        if (Level.Instance.GetPlayer()!=null) transform.position = Vector3.Lerp(transform.position,Level.Instance.GetPlayer().GetPosition() + _startOffset, _speed * Time.deltaTime);
    }
}
