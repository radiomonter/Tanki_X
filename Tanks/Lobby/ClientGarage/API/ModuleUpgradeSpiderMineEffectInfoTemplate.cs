namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d27c27c2c4a6L)]
    public interface ModuleUpgradeSpiderMineEffectInfoTemplate : ModuleUpgradeCommonMineEffectInfoTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectAccelerationPropertyComponent moduleEffectAccelerationProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectSpeedPropertyComponent moduleEffectSpeedProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectTargetingDistancePropertyComponent moduleEffectTargetingDistanceProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectTargetingPeriodPropertyComponent moduleEffectTargetingPeriodProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleSpiderMineEffectChasingEnergyDrainRatePropertyComponent moduleSpiderMineEffectChasingEnergyDrainRateProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleSpiderMineEffectEnergyPropertyComponent moduleSpiderMineEffectEnergyProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleSpiderMineEffectIdleEnergyDrainRatePropertyComponent moduleSpiderMineEffectIdleEnergyDrainRateProperty();
    }
}

