namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c54cd0cc0c91L)]
    public interface ModuleUpgradeEffectWithDurationTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEffectDurationPropertyComponent moduleEffectDurationProperty();
    }
}

