namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15b22c571deL)]
    public class PromoCodeCheckResultEvent : Event
    {
        public string Code { get; set; }

        public PromoCodeCheckResult Result { get; set; }
    }
}

