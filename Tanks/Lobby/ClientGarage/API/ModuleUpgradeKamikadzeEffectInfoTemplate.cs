namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x169e719644dL)]
    public interface ModuleUpgradeKamikadzeEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
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
        ModuleKamikadzeEffectPropertyComponent moduleKamikadzeEFfectProperty();
    }
}

