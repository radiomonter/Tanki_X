namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x16632e31816L)]
    public interface ModuleUpgradeJumpEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        JumpImpactForceMultPropertyComponent jumpImpactForceMultProperty();
        [AutoAdded, PersistentConfig("", false)]
        JumpImpactWorkingTemperaturePropertyComponent jumpImpactWorkingTemperatureProperty();
    }
}

