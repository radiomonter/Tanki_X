namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1566eaa9ab4L)]
    public class UserCountryComponent : Component
    {
        public string CountryCode { get; set; }
    }
}

