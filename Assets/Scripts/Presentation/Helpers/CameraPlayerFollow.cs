using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraPlayerFollow : MonoBehaviour
{
    //private IPlayer _player;
    [SerializeField] private Vector3 _startOffset = new Vector3(-0.6686f, 17.184f, -19.58f);

    private float _speed = 4f; //---------------Sergey: добавил скорость плавного слежения
    

    private void Start()
    {
        //_player = Level.Instance.GetPlayer();
    }

    private void Update()
    {
        //if (_player != null) transform.position = _player.GetPosition() + _startOffset;
        
       
        //------------------------Sergey: плавное слежение за объектом
        if (Level.Instance.GetPlayer()!=null) transform.position = Vector3.Lerp(transform.position,Level.Instance.GetPlayer().GetPosition() + _startOffset, _speed * Time.deltaTime);
    }
}
