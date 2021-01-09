namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;

    [SerialVersionUID(0x160686994d4L)]
    public interface PremiumQuestMarketItemTemplate : GarageItemImagedTemplate, MarketItemTemplate, DurationItemTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("unityAsset", false)]
        AssetReferenceComponent assetReference();
        [AutoAdded, PersistentConfig("", false)]
        CardImageItemComponent cardImageItem();
        [AutoAdded]
        PremiumQuestItemComponent premiumQuestItem();
    }
}

