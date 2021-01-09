namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x16716cbc3b6L)]
    public interface ModuleUpgradeExternalImpactEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectImpactPropertyComponent moduleEffectImpactProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectSplashDamageMinPercentPropertyComponent moduleEffectSplashDamageMinPercentProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectSplashRadiusPropertyComponent moduleEffectSplashRadiusProperty();
        [AutoAdded]
        ModuleExternalImpactEffectPropertyComponent moduleExternalImpactEffectProperty();
    }
}

