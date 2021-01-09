﻿namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1643b74dac6L)]
    public interface GoldBonusMarketItemTemplate : GoldBonusItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
    }
}
