namespace Lobby.ClientPayment.Impl
{
    using System;
    using System.Collections.Generic;

    public class BankCardUtils
    {
        private static Dictionary<BankCardType, HashSet<string>> validStarts = GenerateStarts();
        private static Dictionary<BankCardType, HashSet<int>> validLengths = GenerateLengths();

        private static Dictionary<BankCardType, HashSet<int>> GenerateLengths()
        {
            Dictionary<BankCardType, HashSet<int>> dictionary = new Dictionary<BankCardType, HashSet<int>>();
            HashSet<int> set = new HashSet<int> { 15 };
            dictionary.Add(BankCardType.AMERICAN_EXPRESS, set);
            set = new HashSet<int> { 
                0x10,
                0x11,
                0x12,
                0x13
            };
            dictionary.Add(BankCardType.CHINA_UNIONPAY, set);
            set = new HashSet<int> { 14 };
            dictionary.Add(BankCardType.DINERS_CLUB_CARTE_BLANCHE, set);
            set = new HashSet<int> { 14 };
            dictionary.Add(BankCardType.DINERS_CLUB_INTERNATIONAL, set);
            set = new HashSet<int> { 0x10 };
            dictionary.Add(BankCardType.DINERS_CLUB_UNITED_STATES_AND_CANADA, set);
            set = new HashSet<int> { 
                0x10,
                0x13
            };
            dictionary.Add(BankCardType.DISCOVERY_CARD, set);
            set = new HashSet<int> { 
                0x10,
                0x11,
                0x12,
                0x13
            };
            dictionary.Add(BankCardType.INTERPAYMENT, set);
            set = new HashSet<int> { 0x10 };
            dictionary.Add(BankCardType.INSTAPAYMENT, set);
            set = new HashSet<int> { 0x10 };
            dictionary.Add(BankCardType.JCB, set);
            set = new HashSet<int> { 
                12,
                13,
                14,
                15,
                0x10,
                0x11,
                0x12,
                0x13
            };
            dictionary.Add(BankCardType.MAESTRO, set);
            set = new HashSet<int> { 0x10 };
            dictionary.Add(BankCardType.DANKORT, set);
            set = new HashSet<int> { 0x10 };
            dictionary.Add(BankCardType.NSPK_MIR, set);
            set = new HashSet<int> { 0x10 };
            dictionary.Add(BankCardType.MASTERCARD, set);
            set = new HashSet<int> { 
                13,
                0x10,
                0x13
            };
            dictionary.Add(BankCardType.VISA, set);
            set = new HashSet<int> { 15 };
            dictionary.Add(BankCardType.UATP, set);
            set = new HashSet<int> { 0x10 };
            dictionary.Add(BankCardType.CARDGUARD_EAD_BG_ILS, set);
            return dictionary;
        }

        private static Dictionary<BankCardType, HashSet<string>> GenerateStarts()
        {
            Dictionary<BankCardType, HashSet<string>> dictionary = new Dictionary<BankCardType, HashSet<string>>();
            HashSet<string> set = new HashSet<string> { 
                "34",
                "37"
            };
            dictionary.Add(BankCardType.AMERICAN_EXPRESS, set);
            set = new HashSet<string> { "62" };
            dictionary.Add(BankCardType.CHINA_UNIONPAY, set);
            set = new HashSet<string> { 
                "300",
                "301",
                "302",
                "303",
                "304",
                "305"
            };
            dictionary.Add(BankCardType.DINERS_CLUB_CARTE_BLANCHE, set);
            set = new HashSet<string> { 
                "300",
                "301",
                "302",
                "303",
                "304",
                "305",
                "309",
                "36"
            };
            dictionary.Add(BankCardType.DINERS_CLUB_INTERNATIONAL, set);
            set = new HashSet<string> { 
                "54",
                "55"
            };
            dictionary.Add(BankCardType.DINERS_CLUB_UNITED_STATES_AND_CANADA, set);
            set = new HashSet<string> { 
                "6011",
                "644",
                "645",
                "646",
                "647",
                "648",
                "649",
                "622"
            };
            dictionary.Add(BankCardType.DISCOVERY_CARD, set);
            set = new HashSet<string> { "636" };
            dictionary.Add(BankCardType.INTERPAYMENT, set);
            set = new HashSet<string> { 
                "637",
                "638",
                "639"
            };
            dictionary.Add(BankCardType.INSTAPAYMENT, set);
            HashSet<string> set2 = new HashSet<string>();
            for (int i = 0xdc8; i <= 0xe05; i++)
            {
                set2.Add(i.ToString());
            }
            dictionary.Add(BankCardType.JCB, set2);
            set2 = new HashSet<string>();
            for (int j = 0x38; j <= 0x45; j++)
            {
                set2.Add(j.ToString());
            }
            set2.Add("50");
            dictionary.Add(BankCardType.MAESTRO, set2);
            set = new HashSet<string> { 
                "4175",
                "4571",
                "639"
            };
            dictionary.Add(BankCardType.DANKORT, set);
            set = new HashSet<string> { 
                "2200",
                "2201",
                "2202",
                "2203",
                "2204"
            };
            dictionary.Add(BankCardType.NSPK_MIR, set);
            set2 = new HashSet<string>();
            for (int k = 0x8ad; k <= 0xaa0; k++)
            {
                set2.Add(k.ToString());
            }
            for (int m = 0x33; m <= 0x37; m++)
            {
                set2.Add(m.ToString());
            }
            dictionary.Add(BankCardType.MASTERCARD, set2);
            set = new HashSet<string> { "4" };
            dictionary.Add(BankCardType.VISA, set);
            set = new HashSet<string> { "1" };
            dictionary.Add(BankCardType.UATP, set);
            set = new HashSet<string> { "5392" };
            dictionary.Add(BankCardType.CARDGUARD_EAD_BG_ILS, set);
            return dictionary;
        }

        public static BankCardType GetBankCardType(string number)
        {
            BankCardType type;
            string str = number.Replace(" ", string.Empty);
            using (Dictionary<BankCardType, HashSet<string>>.Enumerator enumerator = validStarts.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        KeyValuePair<BankCardType, HashSet<string>> current = enumerator.Current;
                        using (HashSet<string>.Enumerator enumerator2 = current.Value.GetEnumerator())
                        {
                            while (true)
                            {
                                if (!enumerator2.MoveNext())
                                {
                                    break;
                                }
                                string str2 = enumerator2.Current;
                                if (str.StartsWith(str2) && validLengths[current.Key].Contains(str.Length))
                                {
                                    return current.Key;
                                }
                            }
                        }
                        continue;
                    }
                    else
                    {
                        return BankCardType.INVALID;
                    }
                    break;
                }
            }
            return type;
        }

        public static bool IsBankCard(string number) => 
            GetBankCardType(number) != BankCardType.INVALID;
    }
}

