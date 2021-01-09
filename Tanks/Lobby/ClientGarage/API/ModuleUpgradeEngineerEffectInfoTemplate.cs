namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c856f6065e7cL)]
    public interface ModuleUpgradeEngineerEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEngineerEffectDurationFactorPropertyComponent moduleEngineerEffectDurationFactorProperty();
    }
}

