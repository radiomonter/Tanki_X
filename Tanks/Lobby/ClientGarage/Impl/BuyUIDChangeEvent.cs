namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1575141fda2L)]
    public class BuyUIDChangeEvent : Event
    {
        public string Uid { get; set; }

        public long Price { get; set; }
    }
}

