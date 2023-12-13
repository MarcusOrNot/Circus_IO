using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterVisualForm : MonoBehaviour
{
    public bool IsVisible { get => _isVisible; } 

    public void SetVisiblityStatus(bool isVisible)
    {
        foreach (Renderer visualElement in _visualElements)
        {
            visualElement.enabled = isVisible;
        }
        _isVisible = isVisible;
    }


    protected List<Renderer> _visualElements = new List<Renderer>();


    private bool _isVisible = true;

    protected virtual void Awake()
    {
        _visualElements.AddRange(GetComponentsInChildren<Renderer>(true));   
    }    

}
