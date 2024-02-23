using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressItems
{
    public List<HatType> OpenedHats { get; }
    public void OpenHat(HatType hat);
    public void InitHats(List<HatType> hats);
}
