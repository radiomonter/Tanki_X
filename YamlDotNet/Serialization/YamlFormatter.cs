namespace YamlDotNet.Serialization
{
    using System;
    using System.Globalization;

    internal static class YamlFormatter
    {
        public static readonly NumberFormatInfo NumberFormat;

        static YamlFormatter()
        {
            NumberFormatInfo info = new NumberFormatInfo {
                CurrencyDecimalSeparator = ".",
                CurrencyGroupSeparator = "_"
            };
            info.CurrencyGroupSizes = new int[] { 3 };
            info.CurrencySymbol = string.Empty;
            info.CurrencyDecimalDigits = 0x63;
            info.NumberDecimalSeparator = ".";
            info.NumberGroupSeparator = "_";
            info.NumberGroupSizes = new int[] { 3 };
            info.NumberDecimalDigits = 0x63;
            info.NaNSymbol = ".nan";
            info.PositiveInfinitySymbol = ".inf";
            info.NegativeInfinitySymbol = "-.inf";
            NumberFormat = info;
        }

        public static string FormatBoolean(object boolean) => 
            !boolean.Equals(true) ? "false" : "true";

        public static string FormatDateTime(object dateTime) => 
            ((DateTime) dateTime).ToString("o", CultureInfo.InvariantCulture);

        public static string FormatNumber(double number) => 
            number.ToString("G17", NumberFormat);

        public static string FormatNumber(object number) => 
            Convert.ToString(number, NumberFormat);

        public static string FormatNumber(float number) => 
            number.ToString("G17", NumberFormat);

        public static string FormatTimeSpan(object timeSpan) => 
            ((TimeSpan) timeSpan).ToString();
    }
}

