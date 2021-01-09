namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c792716c9982L)]
    public interface ModuleUpgradeDamageEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleDamageEffectMaxFactorPropertyComponent moduleDamageEffectMaxFactorProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleDamageEffectMinFactorPropertyComponent moduleDamageEffectMinFactorProperty();
    }
}

