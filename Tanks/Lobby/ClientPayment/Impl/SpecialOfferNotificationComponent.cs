namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d43a70054bfcd8L)]
    public class SpecialOfferNotificationComponent : Component
    {
        public string OfferName { get; set; }
    }
}

