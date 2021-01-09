namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15704c26453L)]
    public class GoodsXPriceComponent : Component
    {
        public long Price { get; set; }
    }
}

