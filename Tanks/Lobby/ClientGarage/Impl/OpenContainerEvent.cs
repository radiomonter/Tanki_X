namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x158aa4304bdL)]
    public class OpenContainerEvent : Event
    {
        private long amount = 1L;

        public long Amount
        {
            get => 
                this.amount;
            set => 
                this.amount = value;
        }
    }
}

