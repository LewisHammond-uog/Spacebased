using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which is used for helper functions to format time in to a minutes/seconds/miliseconds format
/// </summary>
public static class TimerHelper {

    /// <summary>
    /// Takes a laptime from a raw seconds input and converts it in to minutes, seconds and miliseconds
    /// </summary>
    /// <param name="laptime"></param>
    /// <returns>Lap Time Formatted as 00 00 00</returns>
    public static string FormatLapTime(float laptime)
    {
        //Calcuate Minutes/Seconds/Milliseconds
        int timeMinutes = Mathf.FloorToInt((laptime) / 60);
        int timeSeconds = Mathf.FloorToInt((laptime) % 60);
        int timeMiliseconds = (int)Math.Floor((laptime - Math.Truncate(laptime)) * 100);

        return ConvertTimeToString(timeMinutes) + "  " + ConvertTimeToString(timeSeconds) + "  " + ConvertTimeToString(timeMiliseconds);
    }

    /// <summary>
    /// Converts a time section as an int to a string
    /// </summary>
    /// <param name="time">Time to convert</param>
    /// <returns>Formatted Time String</returns>
    public static string ConvertTimeToString(int time)
    {
        if (time > 0)
        {
            if (time < 10f)
            {
                return "0" + time;
            }
            else
            {
                return time.ToString();
            }
        }
        else
        {
            return "00";
        }
    }
}
