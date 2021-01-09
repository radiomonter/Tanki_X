namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a40fffacaL)]
    public interface GameplayChestMarketItemTemplate : ContainerItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded]
        GameplayChestItemComponent gameplayChestItem();
        [AutoAdded, PersistentConfig("", false)]
        TargetTierComponent targetTier();
    }
}

