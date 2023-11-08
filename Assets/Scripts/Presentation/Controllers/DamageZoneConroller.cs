using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageZoneConroller : MonoBehaviour
{
    [SerializeField] private Transform _visual;
    public int StartSize;
    private Vector3 _startPos;
    private float _currentSize;
    private void Awake()
    {
        //_currentSize = StartSize;
        SetSize(StartSize);
        _startPos = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_startPos, Level.Instance.GetPlayer().GetPosition()) > _currentSize*3)
        {
            Debug.Log("char is outside");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SetSize(_currentSize / 2);
        }
    }

    private void SetSize(float size)
    {
        _currentSize = size;
        _visual.transform.localScale = new Vector3(size, 1, size);
    }
}
