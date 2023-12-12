using System;
using System.Collections.Generic;
using UnityEngine;


public class KaufmoForm : HunterVisualForm, ICanAttack
{
    public void SendAttackEvent()
    {
        foreach (var item in _onAttack) item?.Invoke();
    }

    public void SetOnAttack(Action onAttack) { _onAttack.Add(onAttack); }
    private List<Action> _onAttack = new List<Action>();
       

    private Renderer _mainMesh = null;    
    private INeedKaufmoMaterial[] _coloredElements = null;


    protected override void Awake()
    {        
        base.Awake();
        _mainMesh = GetComponentInChildren<SkinnedMeshRenderer>(true);     
        _coloredElements = GetComponentsInChildren<INeedKaufmoMaterial>(true);
    }

    private void Start()
    {            
        foreach (INeedKaufmoMaterial coloredElement in _coloredElements) coloredElement.Material = _mainMesh.sharedMaterials[0];        
    }

    private void OnDestroy()
    {
        _onAttack.Clear();
    }

}
