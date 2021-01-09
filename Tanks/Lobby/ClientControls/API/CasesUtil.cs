namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientLocale.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class CasesUtil
    {
        public static CaseType GetCase(int count)
        {
            string twoLetterISOLanguageName = LocaleUtils.GetCulture().TwoLetterISOLanguageName;
            if (twoLetterISOLanguageName != null)
            {
                if (twoLetterISOLanguageName == "ru")
                {
                    return GetRUCase(count);
                }
                if (twoLetterISOLanguageName == "en")
                {
                    return GetENCase(count);
                }
            }
            return CaseType.DEFAULT;
        }

        public static Dictionary<CaseType, string> GetDictionary(string localizedVariants)
        {
            Dictionary<CaseType, string> dictionary = new Dictionary<CaseType, string>();
            char[] separator = new char[] { ' ' };
            foreach (string str in localizedVariants.Split(separator))
            {
                char[] chArray2 = new char[] { ':' };
                CaseType key = (CaseType) Enum.Parse(typeof(CaseType), str.Split(chArray2)[0]);
                char[] chArray3 = new char[] { ':' };
                string str2 = str.Split(chArray3)[1];
                dictionary.Add(key, str2);
            }
            return dictionary;
        }

        public static CaseType GetENCase(int count) => 
            (count != 1) ? CaseType.DEFAULT : CaseType.ONE;

        public static string GetLocalizedCase(string localizedVariants, int count)
        {
            string str = localizedVariants;
            try
            {
                str = GetDictionary(localizedVariants)[GetCase(count)];
            }
            catch (Exception)
            {
                Debug.LogError("Check string: " + localizedVariants);
            }
            return str;
        }

        public static CaseType GetRUCase(int count) => 
            ((count <= 10) || (count >= 20)) ? (((count % 10) != 1) ? ((((count % 10) == 2) || (((count % 10) == 3) || ((count % 10) == 4))) ? CaseType.TWO : CaseType.DEFAULT) : CaseType.ONE) : CaseType.DEFAULT;
    }
}

