namespace Tanks.Lobby.ClientPayment.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public class SaleState
    {
        public SaleState()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.PriceMultiplier = 1.0;
            this.AmountMultiplier = 1.0;
        }

        public double PriceMultiplier { get; set; }

        public double AmountMultiplier { get; set; }
    }
}

