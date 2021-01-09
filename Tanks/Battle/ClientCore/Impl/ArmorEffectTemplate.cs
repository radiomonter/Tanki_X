namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x159fd9f09f6L)]
    public interface ArmorEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        ArmorEffectComponent armorEffect();
        [PersistentConfig("", false)]
        DurationConfigComponent duration();
    }
}

