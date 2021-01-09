namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4fb4c5cc04099L)]
    public class EnergyContextBuyEvent : Event
    {
        public EnergyContextBuyEvent()
        {
        }

        public EnergyContextBuyEvent(long count, long price)
        {
            this.Count = count;
            this.XPrice = price;
        }

        public long XPrice { get; set; }

        public long Count { get; set; }
    }
}

