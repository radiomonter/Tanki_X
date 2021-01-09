namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(-5630755063511713066L)]
    public interface MapTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        AssetReferenceComponent assetReference();
        AssetRequestComponent assetRequest();
        [AutoAdded, PersistentConfig("", false)]
        DescriptionItemComponent descriptionItem();
        [AutoAdded, PersistentConfig("", false)]
        FlavorListComponent flavorList();
        MapComponent map();
        MapInstanceComponent mapInstance();
        [AutoAdded, PersistentConfig("", false)]
        MapLoadPreviewComponent mapLoadPreview();
        [AutoAdded, PersistentConfig("", false)]
        MapModeRestrictionComponent mapModeRestriction();
        [AutoAdded, PersistentConfig("", false)]
        MapPreviewComponent mapPreview();
    }
}

