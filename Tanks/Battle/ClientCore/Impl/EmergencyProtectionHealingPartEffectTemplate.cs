namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4d02ee5740e91L)]
    public interface EmergencyProtectionHealingPartEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        EmergencyProtectionHealingPartEffectComponent emergencyProtectionHealingPartEffect();
    }
}

