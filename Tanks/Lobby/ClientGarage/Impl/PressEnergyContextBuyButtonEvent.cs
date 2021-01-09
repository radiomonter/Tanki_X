namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PressEnergyContextBuyButtonEvent : Event
    {
        public PressEnergyContextBuyButtonEvent()
        {
        }

        public PressEnergyContextBuyButtonEvent(long count, long price)
        {
            this.Count = count;
            this.XPrice = price;
        }

        public long XPrice { get; set; }

        public long Count { get; set; }
    }
}

