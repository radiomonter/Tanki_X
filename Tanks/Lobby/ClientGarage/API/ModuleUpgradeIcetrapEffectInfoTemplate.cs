namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4e48331ca14feL)]
    public interface ModuleUpgradeIcetrapEffectInfoTemplate : ModuleUpgradeMineEffectInfoTemplate, ModuleUpgradeCommonMineEffectInfoTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleIcetrapEffectTemperatureDeltaPropertyComponent moduleIcetrapEffectTemperatureDeltaProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleIcetrapEffectTemperatureDurationPropertyComponent moduleIcetrapEffectTemperatureDurationProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleIcetrapEffectTemperatureLimitPropertyComponent moduleIcetrapEffectTemperatureLimitProperty();
    }
}

