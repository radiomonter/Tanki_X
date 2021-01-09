namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d4d2e30852e0L)]
    public interface ModuleUpgradeEnergyInjectionEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEnergyInjectionEffectReloadPercentPropertyComponent moduleEnergyInjectionEffectReloadPercentProperty();
    }
}

