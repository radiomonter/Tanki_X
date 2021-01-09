namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x8d3e24f2425f648L)]
    public interface GraffitiBattleItemTemplate : Template
    {
        [AutoAdded, PersistentConfig("unityAsset", false)]
        AssetReferenceComponent assetReference();
        [AutoAdded]
        AssetRequestComponent assetRequest();
        [AutoAdded, PersistentConfig("", false)]
        ImageItemComponent imageItem();
        [AutoAdded, PersistentConfig("", false)]
        ItemRarityComponent itemRarity();
    }
}

