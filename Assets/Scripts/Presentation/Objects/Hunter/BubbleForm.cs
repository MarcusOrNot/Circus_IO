
using Zenject;
using UnityEngine;


public class BubbleForm : HunterVisualForm, ILoveHat
{
    [Inject] private HatFactory _hatFactory;
    [SerializeField] private GameObject _slotForHat = null;
    private Hat _currentHat = null;
    public void SetHat(HatType hatType)
    {
        if (_slotForHat == null) return;
        if (_currentHat !=null) Destroy(_currentHat.gameObject);
        Hat hat = _hatFactory.Spawn(hatType);
        if (hat == null) return;
        _currentHat = hat;
        hat.transform.SetParent(_slotForHat.transform, false);
        _visualElements.AddRange(hat.gameObject.GetComponentsInChildren<Renderer>(true));
    }
    
}
