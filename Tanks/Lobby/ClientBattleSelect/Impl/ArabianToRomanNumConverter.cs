namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;

    public static class ArabianToRomanNumConverter
    {
        private static readonly int[] arabian = new int[] { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 0x3e8 };
        private static readonly string[] roman;

        static ArabianToRomanNumConverter()
        {
            string[] textArray1 = new string[13];
            textArray1[0] = "I";
            textArray1[1] = "IV";
            textArray1[2] = "V";
            textArray1[3] = "IX";
            textArray1[4] = "X";
            textArray1[5] = "XL";
            textArray1[6] = "L";
            textArray1[7] = "XC";
            textArray1[8] = "C";
            textArray1[9] = "CD";
            textArray1[10] = "D";
            textArray1[11] = "CM";
            textArray1[12] = "M";
            roman = textArray1;
        }

        public static string ArabianToRoman(int arabianNum)
        {
            string str = string.Empty;
            int index = arabian.Length - 1;
            while (arabianNum > 0)
            {
                if (arabianNum < arabian[index])
                {
                    index--;
                    continue;
                }
                str = str + roman[index];
                arabianNum -= arabian[index];
            }
            return str;
        }

        public static int RomanToArabian(string romanNum)
        {
            romanNum = romanNum.ToUpper();
            int num = 0;
            int index = arabian.Length - 1;
            int startIndex = 0;
            while ((index >= 0) && (startIndex < romanNum.Length))
            {
                if (romanNum.Substring(startIndex, roman[index].Length) != roman[index])
                {
                    index--;
                    continue;
                }
                num += arabian[index];
                startIndex += roman[index].Length;
            }
            return num;
        }
    }
}

