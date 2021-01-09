namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1594106dfeeL)]
    public class PriceChangedEvent : Event
    {
        public long OldPrice { get; set; }

        public long Price { get; set; }

        public long OldXPrice { get; set; }

        public long XPrice { get; set; }
    }
}

