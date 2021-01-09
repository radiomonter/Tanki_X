namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d049115a25e4L)]
    public interface ModuleUpgradeRageEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleRageEffectReduceCooldownTimePerKillPropertyComponent moduleRageEffectReduceCooldownTimePerKillProperty();
    }
}

