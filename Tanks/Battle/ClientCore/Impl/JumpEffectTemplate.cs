namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x16632de9a22L)]
    public interface JumpEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        JumpEffectComponent jumpEffect();
    }
}

