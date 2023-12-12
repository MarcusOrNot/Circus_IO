


using UnityEngine;

public class PacmanForm : HunterVisualForm, ILoveHat
{
    [SerializeField] private GameObject _slotForHat = null;

    public void PutOnHat(Hat hat)
    {
        if (_slotForHat == null) return;
        hat.gameObject.transform.SetParent(_slotForHat.transform);
    }
}
