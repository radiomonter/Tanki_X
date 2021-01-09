namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15e947ffc97L)]
    public interface ModuleUpgradeForcefieldEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded]
        ForceFieldModuleComponent forceFieldModule();
    }
}

