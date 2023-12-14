
using Zenject;
using UnityEngine;


public class BubbleForm : HunterVisualForm, ILoveHat
{

    public void SetHat(HatType hatType)
    {
        if (_slotForHat == null) return;
        Hat hat = _hatFactory.Spawn(hatType);
        if (hat == null) return;
        hat.transform.SetParent(_slotForHat.transform, false);
        _visualElements.AddRange(hat.gameObject.GetComponentsInChildren<Renderer>(true));
    }



    [Inject] private HatFactory _hatFactory;

    [SerializeField] private GameObject _slotForHat = null;
    
}
