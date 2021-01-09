namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d8bfa533ff40L)]
    public interface ModuleUpgradeEMPEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEMPEffectRadiusPropertyComponent moduleEMPEffectRadiusProperty();
    }
}

