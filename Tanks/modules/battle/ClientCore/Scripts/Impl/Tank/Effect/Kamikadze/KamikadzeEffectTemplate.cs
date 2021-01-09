namespace tanks.modules.battle.ClientCore.Scripts.Impl.Tank.Effect.Kamikadze
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x169e247a0aaL)]
    public interface KamikadzeEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        KamikadzeEffectComponent kamikadzeEffect();
    }
}

