namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a37669fd5L)]
    public interface HealingEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        HealingEffectComponent healingEffect();
    }
}

