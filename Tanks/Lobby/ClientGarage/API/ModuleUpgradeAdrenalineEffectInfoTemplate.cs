namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d364f76e7a2eL)]
    public interface ModuleUpgradeAdrenalineEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleAdrenalineEffectCooldownSpeedCoeffPropertyComponent moduleAdrenalineEffectCooldownSpeedCoeffProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleAdrenalineEffectMaxHPPercentWorkingPropertyComponent moduleAdrenalineEffectMaxHPPercentWorkingProperty();
    }
}

