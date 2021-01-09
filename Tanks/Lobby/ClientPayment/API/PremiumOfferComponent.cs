namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x1606e1f3011L)]
    public class PremiumOfferComponent : Component
    {
        public int MinRank { get; set; }

        public int MaxRank { get; set; }
    }
}

