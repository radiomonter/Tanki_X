namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MarketItemSale
    {
        public int salePercent { get; set; }

        public int priceOffset { get; set; }

        public int xPriceOffset { get; set; }

        public Date endDate { get; set; }
    }
}

