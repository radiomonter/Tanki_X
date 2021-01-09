namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ItemAutoIncreasePriceComponent : Component
    {
        public int StartCount { get; set; }

        public int PriceIncreaseAmount { get; set; }

        public int MaxAdditionalPrice { get; set; }
    }
}

