namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientLocale.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class IntegerExtensions
    {
        public static string ToStringSeparatedByThousands(this double value) => 
            (Math.Abs((double) (value - ((int) value))) >= Mathf.Epsilon) ? value.ToString("N2", LocaleUtils.GetCulture()) : value.ToString("N0", LocaleUtils.GetCulture());

        public static string ToStringSeparatedByThousands(this int value) => 
            value.ToString("N0", LocaleUtils.GetCulture());

        public static string ToStringSeparatedByThousands(this long value) => 
            value.ToString("N0", LocaleUtils.GetCulture());
    }
}

