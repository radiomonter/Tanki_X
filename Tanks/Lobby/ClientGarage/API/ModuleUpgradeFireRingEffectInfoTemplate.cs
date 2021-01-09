namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API.FireTrap;

    [SerialVersionUID(0x1673006f25dL)]
    public interface ModuleUpgradeFireRingEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
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
        ModuleFireRingEffectPropertyComponent moduleFireRingEffectProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleIcetrapEffectTemperatureDeltaPropertyComponent moduleIcetrapEffectTemperatureDeltaProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleIcetrapEffectTemperatureDurationPropertyComponent moduleIcetrapEffectTemperatureDurationProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleIcetrapEffectTemperatureLimitPropertyComponent moduleIcetrapEffectTemperatureLimitProperty();
    }
}

