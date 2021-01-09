namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class OrdinalNumbersSuffixTextComponent : LocalizedControl
    {
        public string GetSuffix(int number)
        {
            int num = number % 100;
            int num3 = num % 10;
            if ((num / 10) != 1)
            {
                switch (num3)
                {
                    case 1:
                        return this.StSuffix;

                    case 2:
                        return this.NdSuffix;

                    case 3:
                        return this.RdSuffix;
                }
            }
            return this.ThSuffix;
        }

        public string StSuffix { get; set; }

        public string NdSuffix { get; set; }

        public string RdSuffix { get; set; }

        public string ThSuffix { get; set; }
    }
}

