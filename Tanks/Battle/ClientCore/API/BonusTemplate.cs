namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x68d513112f429f1aL)]
    public interface BonusTemplate : Template
    {
        [PersistentConfig("", false), AutoAdded]
        BonusBoxPrefabComponent bonusBoxPrefab();
        [PersistentConfig("", false), AutoAdded]
        BonusConfigComponent bonusConfig();
        BonusRegionGroupComponent bonusRegionGroup();
    }
}

