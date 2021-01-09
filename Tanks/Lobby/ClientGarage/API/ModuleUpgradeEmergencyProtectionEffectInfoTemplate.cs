namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d01ec2993789L)]
    public interface ModuleUpgradeEmergencyProtectionEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleEmergencyProtectionEffectAdditiveHPFactorPropertyComponent moduleEmergencyProtectionEffectAdditiveHPFactorProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEmergencyProtectionEffectFixedHPPropertyComponent moduleEmergencyProtectionEffectFixedHPProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleEmergencyProtectionEffectHolyshieldDurationPropertyComponent moduleEmergencyProtectionEffectHolyshieldDurationProperty();
    }
}

