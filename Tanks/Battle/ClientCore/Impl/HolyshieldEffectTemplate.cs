namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4be297161ecd7L)]
    public interface HolyshieldEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        HolyshieldEffectComponent holyshieldEffect();
    }
}

