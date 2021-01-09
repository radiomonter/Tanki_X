namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using System.Runtime.CompilerServices;

    public class MarketItemBundle
    {
        private int amount = 1;

        public long MarketItem { get; set; }

        public int Amount
        {
            get => 
                this.amount;
            set => 
                this.amount = value;
        }

        public int Max { get; set; }
    }
}

