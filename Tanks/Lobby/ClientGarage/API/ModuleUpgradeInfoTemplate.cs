namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x8d4c53d19d5f5c8L)]
    public interface ModuleUpgradeInfoTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleAmmunitionPropertyComponent moduleAmmunitionProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleCooldownPropertyComponent moduleCooldownProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleVisualPropertiesComponent moduleVisualProperties();
    }
}

