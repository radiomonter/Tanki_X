namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d195d09e00bda1L)]
    public interface ModuleUpgradeDroneEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectActivationTimePropertyComponent moduleEffectActivationTimeProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectTargetingDistancePropertyComponent moduleEffectTargetingDistanceProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectTargetingPeriodPropertyComponent moduleEffectTargetingPeriodProperty();
    }
}

