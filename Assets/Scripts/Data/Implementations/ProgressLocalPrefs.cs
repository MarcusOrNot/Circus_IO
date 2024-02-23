using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLocalPrefs : IProgressItems
{
    private const string HAT_PROGRESS = "Progress_hat";

    public List<HatType> OpenedHats
    {
        get
        {
            if (PlayerPrefs.GetString(HAT_PROGRESS, "") != "")
                return JsonUtility.FromJson<ContainerHats>(PlayerPrefs.GetString(HAT_PROGRESS, "")).Hats;
            return new List<HatType>();
        }
    }

    public void InitHats(List<HatType> hats)
    {
        PlayerPrefs.SetString(HAT_PROGRESS, JsonUtility.ToJson(new ContainerHats(hats)));
    }

    public void OpenHat(HatType hat)
    {
        var opened = OpenedHats;
        opened.Add(hat);
        PlayerPrefs.SetString(HAT_PROGRESS, JsonUtility.ToJson(new ContainerHats(opened)));
    }






    [Serializable]
    private class ContainerHats
    {
        public List<HatType> Hats = new List<HatType>();
        public ContainerHats(List<HatType> hats)
        {
            Hats = hats;
        }
    }
}
