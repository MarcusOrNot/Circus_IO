using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLocalPrefs : IData
{
    private const string INTERDATA = "data_inter_time";
    public DateTime InterstitialDate { 
        get {
            var time_string = PlayerPrefs.GetString(INTERDATA,"");
            if (time_string == "") return DateTime.MinValue;
            else return Utils.DateFromString(time_string);
        }
        set => PlayerPrefs.SetString(INTERDATA, Utils.DateToString(value)); 
    }
}
