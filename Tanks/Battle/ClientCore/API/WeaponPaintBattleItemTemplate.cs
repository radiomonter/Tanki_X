namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;

    [SerialVersionUID(0x8d48bc944c2257fL)]
    public interface WeaponPaintBattleItemTemplate : Template
    {
        [AutoAdded, PersistentConfig("unityAsset", false)]
        AssetReferenceComponent assetReference();
        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}

