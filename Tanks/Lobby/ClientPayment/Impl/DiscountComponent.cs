namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8ee20dba8ba4ee5L)]
    public class DiscountComponent : Component
    {
        public float DiscountCoeff { get; set; }
    }
}

