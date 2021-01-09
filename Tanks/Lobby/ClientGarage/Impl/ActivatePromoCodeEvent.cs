namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15b1f3839beL)]
    public class ActivatePromoCodeEvent : Event
    {
        public string Code { get; set; }
    }
}

