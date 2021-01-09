namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c8487a7f34c5L)]
    public interface ModuleUpgradeTempblockEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleTempblockDecrementPropertyComponent moduleTempblockDecrementProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleTempblockIncrementPropertyComponent moduleTempblockIncrementProperty();
    }
}

