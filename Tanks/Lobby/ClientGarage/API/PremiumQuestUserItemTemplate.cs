namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;

    [SerialVersionUID(0x160688695e4L)]
    public interface PremiumQuestUserItemTemplate : GarageItemImagedTemplate, UserItemTemplate, DurationItemTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("unityAsset", false)]
        AssetReferenceComponent assetReference();
        [AutoAdded]
        PremiumQuestItemComponent premiumQuestItem();
    }
}

