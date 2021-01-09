namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d450e3443502bbL)]
    public interface InvisibilityEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        InvisibilityEffectComponent invisibilityEffect();
    }
}

