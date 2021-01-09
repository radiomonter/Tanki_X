namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x6c7b9ae2e74a417L)]
    public class LegendaryTankSpecialOfferComponent : Component
    {
        public RentTankRole TankRole { get; set; }

        public long MaxRank { get; set; }
    }
}

