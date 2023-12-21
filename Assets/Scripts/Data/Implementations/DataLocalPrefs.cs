using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLocalPrefs : IData
{
    private const string AD_DATE = "data_ad_time";
    public DateTime LastAdDate { 
        get {
            var time_string = PlayerPrefs.GetString(AD_DATE,"");
            if (time_string == "") return DateTime.MinValue;
            else return Utils.DateFromString(time_string);
        }
        set => PlayerPrefs.SetString(AD_DATE, Utils.DateToString(value)); 
    }
}
