namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x15a36561acfL)]
    public interface TurboSpeedEffectTemplate : EffectBaseTemplate, Template
    {
        [PersistentConfig("", false)]
        DurationConfigComponent duration();
        [AutoAdded]
        TurboSpeedEffectComponent turboSpeedEffect();
    }
}

