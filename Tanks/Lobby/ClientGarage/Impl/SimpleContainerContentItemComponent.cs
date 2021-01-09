namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SimpleContainerContentItemComponent : Component
    {
        public long MarketItemId { get; set; }

        public string NameLokalizationKey { get; set; }
    }
}

