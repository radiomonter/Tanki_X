namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4b24dda06dae4L)]
    public interface ModuleTierConfigTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig();
    }
}

