namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PriceItemComponent : Component
    {
        public int OldPrice { get; set; }

        public int Price { get; set; }

        [ProtocolTransient]
        public bool IsBuyable =>
            this.Price > 0;
    }
}

