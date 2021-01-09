namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c8390f29bb0aL)]
    public interface OverhaulEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        HealingEffectComponent healingEffect();
    }
}

