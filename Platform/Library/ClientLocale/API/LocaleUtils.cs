namespace Platform.Library.ClientLocale.API
{
    using System;
    using System.Globalization;
    using UnityEngine;

    public class LocaleUtils
    {
        public static string LOCALE_SETTING_NAME = "locale";

        public static CultureInfo GetCulture()
        {
            string savedLocaleCode = GetSavedLocaleCode();
            if (savedLocaleCode != null)
            {
                if (savedLocaleCode == "en")
                {
                    return new CultureInfo("en-US");
                }
                if (savedLocaleCode == "tr")
                {
                    return new CultureInfo("tr-TR");
                }
            }
            return new CultureInfo("ru-RU");
        }

        public static string GetSavedLocaleCode() => 
            PlayerPrefs.GetString(LOCALE_SETTING_NAME);

        public static void SaveLocaleCode(string code)
        {
            PlayerPrefs.SetString(LOCALE_SETTING_NAME, code);
        }
    }
}

