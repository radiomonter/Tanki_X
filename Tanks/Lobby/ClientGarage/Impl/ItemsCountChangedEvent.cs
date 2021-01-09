namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158ce5ef679L)]
    public class ItemsCountChangedEvent : Event
    {
        public long Delta { get; set; }
    }
}

