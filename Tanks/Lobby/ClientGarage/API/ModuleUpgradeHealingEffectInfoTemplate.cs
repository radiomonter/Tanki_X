namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c7a23e5c02aaL)]
    public interface ModuleUpgradeHealingEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleHealingEffectFirstTickMSPropertyComponent moduleHealingEffectFirstTickMsProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleHealingEffectHPPerMSPropertyComponent moduleHealingEffectHPPerMSProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleHealingEffectInstantHPPropertyComponent moduleHealingEffectInstantHPProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleHealingEffectPeriodicTickPropertyComponent moduleHealingEffectPeriodicTickProperty();
    }
}

