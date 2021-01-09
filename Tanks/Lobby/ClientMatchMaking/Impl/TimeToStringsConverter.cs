namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using System;
    using UnityEngine;

    public class TimeToStringsConverter : MonoBehaviour
    {
        public static string MillisecondsToTimerFormat(double milliseconds) => 
            SecondsToTimerFormat(milliseconds / 1000.0);

        public static string SecondsToTimerFormat(double seconds)
        {
            int num = (int) (seconds / 60.0);
            int num2 = ((int) seconds) - (num * 60);
            return $"{num:0}:{num2:00}";
        }
    }
}

