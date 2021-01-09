namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14d09a48751L)]
    public interface BonusRegionAssetsTemplate : Template
    {
        [PersistentConfig("", false), AutoAdded]
        BonusRegionAssetsComponent bonusRegionAssets();
    }
}

