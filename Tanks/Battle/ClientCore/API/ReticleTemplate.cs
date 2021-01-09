namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x8d3e24f02749f68L)]
    public interface ReticleTemplate : Template
    {
        [AutoAdded, PersistentConfig("unityAsset", false)]
        AssetReferenceComponent assetReference();
        [AutoAdded]
        AssetRequestComponent assetRequest();
        [AutoAdded]
        ReticleComponent reticle();
    }
}

