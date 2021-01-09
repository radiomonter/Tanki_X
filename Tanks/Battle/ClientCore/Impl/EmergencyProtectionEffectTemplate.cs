namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4993fb76444f5L)]
    public interface EmergencyProtectionEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        EmergencyProtectionEffectComponent emergencyProtectionEffect();
    }
}

