namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4bd3ee46e1026L)]
    public interface LifestealEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        LifestealEffectComponent lifestealEffect();
    }
}

