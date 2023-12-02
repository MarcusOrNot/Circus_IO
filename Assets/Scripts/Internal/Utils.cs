using System;
using System.Collections;
using System.Collections.Generic;

public static class Utils
{
    public static DateTime DateFromString(string base_string)
    {
        return DateTime.ParseExact(base_string, "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string DateToString(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
    }
    public static double GetSecondsElapsed(DateTime fromDate)
    {
        return (DateTime.Now - fromDate).TotalSeconds;
    }
}
