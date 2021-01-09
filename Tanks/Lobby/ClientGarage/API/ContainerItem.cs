namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ContainerItem
    {
        public List<MarketItemBundle> ItemBundles { get; set; }

        public long Compensation { get; set; }

        public string NameLocalizationKey { get; set; }
    }
}

