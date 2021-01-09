namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GoldBonusOfferComponent : Component
    {
        public long Count { get; set; }
    }
}

