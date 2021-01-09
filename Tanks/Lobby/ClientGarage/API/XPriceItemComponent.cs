namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class XPriceItemComponent : Component
    {
        public int Pieces { get; set; }

        public int OldPrice { get; set; }

        public int Price { get; set; }

        [ProtocolTransient]
        public bool IsBuyable =>
            this.Price > 0;
    }
}

