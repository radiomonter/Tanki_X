namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d427a0f48bf802L)]
    public class SpecialOfferDurationComponent : Component
    {
        public bool OneShot { get; set; }
    }
}

