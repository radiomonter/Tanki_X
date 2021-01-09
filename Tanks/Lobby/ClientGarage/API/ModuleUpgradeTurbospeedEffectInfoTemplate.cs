namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c78cf16e6a6aL)]
    public interface ModuleUpgradeTurbospeedEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleTurbospeedEffectPropertyComponent moduleTurbospeedEffectProperty();
    }
}

