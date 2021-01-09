namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BundleContainerContentItemComponent : Component
    {
        public List<MarketItemBundle> MarketItems { get; set; }

        public string NameLokalizationKey { get; set; }
    }
}

