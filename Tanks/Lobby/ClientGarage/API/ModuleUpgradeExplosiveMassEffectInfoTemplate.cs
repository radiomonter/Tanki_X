namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1675ed2e7f7L)]
    public interface ModuleUpgradeExplosiveMassEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectActivationTimePropertyComponent moduleEffectActivationTimeProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectTargetingDistancePropertyComponent moduleEffectTargetingDistanceProperty();
        [AutoAdded]
        ModuleExplosiveMassEffectPropertyComponent moduleExplosiveMassEffectProperty();
    }
}

