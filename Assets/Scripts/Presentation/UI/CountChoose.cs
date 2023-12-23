using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountChoose : MonoBehaviour
{
    [SerializeField] private ElemChoose _choosingPrefab;
    [SerializeField] private int _count = 5;
    //private List<ElemChoose> _elemsList = new List<ElemChoose>();
    private Action<int> _onChoose;
    private int _currentCount = 0;
    public void SetCount(int count)
    {
        int elemSize = (int) (GetComponent<RectTransform>().sizeDelta.y / 1.5f);
        _count = count;
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i));
        for (int i=0; i < _count; i++)
        {
            int pos = i;
            var current = Instantiate(_choosingPrefab, this.transform).GetComponent<ElemChoose>();
            current.Chosen = false;
            current.GetComponent<RectTransform>().sizeDelta = new Vector2(elemSize, elemSize);
            current.SetOnClick(() => {
                for (int i = 0; i < _count; i++)
                {
                    transform.GetChild(i).GetComponent<ElemChoose>().Chosen = (i <= pos);
                }
                _currentCount = pos + 1;
                _onChoose?.Invoke(_currentCount); 
            });
        }
    }
    public void SetOnChoose(Action<int> onChoose)
    {
        _onChoose = onChoose;
    }

    public int Count => _currentCount;
    /*private void ChosenElem(int pos)
    {
        for (int i=0; i< _count; i++)
        {
            
        }
    }*/
}
