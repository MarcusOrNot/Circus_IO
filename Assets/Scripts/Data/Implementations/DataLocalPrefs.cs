using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLocalPrefs : IData
{
    private const string AD_DATE = "data_ad_time";
    private const string FEED_VALUE = "data_feed_value";
    public DateTime LastAdDate { 
        get {
            var time_string = PlayerPrefs.GetString(AD_DATE,"");
            if (time_string == "") return DateTime.MinValue;
            else return Utils.DateFromString(time_string);
        }
        set => PlayerPrefs.SetString(AD_DATE, Utils.DateToString(value)); 
    }

    public int FeedValue { get => PlayerPrefs.GetInt(FEED_VALUE, 0); set => PlayerPrefs.SetInt(FEED_VALUE, value); }
}
