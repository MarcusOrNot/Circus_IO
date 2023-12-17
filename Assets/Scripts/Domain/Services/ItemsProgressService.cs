using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemsProgressService
{
    private IProgressItems _progress;
    private List<HatType> _cachedHats = new List<HatType>();
    [Inject]
    public ItemsProgressService(IProgressItems itemsProgress)
    {
        _progress = itemsProgress;
        RefreshHats();
    }
    public bool IsHatOpened(HatType hat)
    {
        return _cachedHats.Contains(hat);
    }
    public void OpenHat(HatType hat)
    {
        _progress.OpenHat(hat);
        RefreshHats();
    }
    private void RefreshHats()
    {
        _cachedHats = _progress.OpenedHats;
    }
}
