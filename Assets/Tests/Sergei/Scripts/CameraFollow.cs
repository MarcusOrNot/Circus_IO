using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Character _character;
    private Vector3 _startOffset = new Vector3(-0.6686f, 17.184f, -19.58f);


    void Start()
    {        
        //if (_character != null) _startOffset = transform.position - _character.transform.position;
    }


    void Update()
    {
        //if (_character != null) transform.position = _character.transform.position + _startOffset;
        if (_character != null) transform.position = _character.transform.position + _startOffset;
    }
}
