namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;

    [SerialVersionUID(0x8d3b1867e4510ffL)]
    public interface HullSkinBattleItemTemplate : Template
    {
        [AutoAdded, PersistentConfig("unityAsset", false)]
        AssetReferenceComponent assetReference();
        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}

