namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class UIExtensions
    {
        private static string GetHex(int num) => 
            "0123456789ABCDEF"[num].ToString();

        public static string ToHexString(this Color color)
        {
            int num = (int) (color.r * 255f);
            int num2 = (int) (color.g * 255f);
            int num3 = (int) (color.b * 255f);
            string[] textArray1 = new string[] { GetHex(num / 0x10), GetHex(num % 0x10), GetHex(num2 / 0x10), GetHex(num2 % 0x10), GetHex(num3 / 0x10), GetHex(num3 % 0x10) };
            return string.Concat(textArray1);
        }
    }
}

