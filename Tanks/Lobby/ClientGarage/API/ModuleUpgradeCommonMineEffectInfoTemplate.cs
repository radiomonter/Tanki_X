namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d27b83ee2aaaL)]
    public interface ModuleUpgradeCommonMineEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectActivationTimePropertyComponent moduleEffectActivationTimeProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleLimitBundleEffectCountPropertyComponent moduleLimitBundleEffectCountProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectBeginHideDistancePropertyComponent moduleMineEffectBeginHideDistanceProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectHideRangePropertyComponent moduleMineEffectHideRangeProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectImpactPropertyComponent moduleMineEffectImpactProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectSplashDamageMaxRadiusPropertyComponent moduleMineEffectSplashDamageMaxRadiusProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectSplashDamageMinPercentPropertyComponent moduleMineEffectSplashDamageMinPercentProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectSplashDamageMinRadiusPropertyComponent moduleMineEffectSplashDamageMinRadiusProperty();
    }
}

