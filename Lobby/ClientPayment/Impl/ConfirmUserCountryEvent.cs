namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x155244cf4adL)]
    public class ConfirmUserCountryEvent : Event
    {
        public string CountryCode { get; set; }
    }
}

