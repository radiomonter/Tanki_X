using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using System;
using System.Runtime.CompilerServices;

public class MarketItemSaleComponent : Component
{
    public int salePercent { get; set; }

    public int priceOffset { get; set; }

    public int xPriceOffset { get; set; }

    public Date endDate { get; set; }
}

