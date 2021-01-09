namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x159fd9f49f0L)]
    public interface DamageEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        DamageEffectComponent damageEffect();
        [PersistentConfig("", false)]
        DurationConfigComponent duration();
    }
}

