namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(-1911920453295891173L)]
    public interface CTFTemplate : TeamBattleTemplate, BattleTemplate, Template
    {
        [PersistentConfig("unityAsset", false)]
        AssetReferenceComponent assetReference();
        AssetRequestComponent assetRequest();
        CTFComponent ctfComponent();
        [PersistentConfig("ctfConfig", false), AutoAdded]
        CTFConfigComponent CTFConfig();
    }
}

