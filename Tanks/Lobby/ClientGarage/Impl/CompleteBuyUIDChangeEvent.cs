namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15793878632L)]
    public class CompleteBuyUIDChangeEvent : Event
    {
        public bool Success { get; set; }
    }
}

