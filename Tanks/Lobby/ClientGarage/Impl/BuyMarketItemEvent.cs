namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15383b143efL)]
    public class BuyMarketItemEvent : Event
    {
        private int amount = 1;

        public int Price { get; set; }

        public int Amount
        {
            get => 
                this.amount;
            set => 
                this.amount = value;
        }
    }
}

