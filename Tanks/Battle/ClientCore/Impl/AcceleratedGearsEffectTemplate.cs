namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a36d63f38L)]
    public interface AcceleratedGearsEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        AcceleratedGearsEffectComponent acceleratedGearsEffect();
    }
}

