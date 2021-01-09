namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15e04887a4cL)]
    public interface ForceFieldEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ForceFieldEffectComponent forceFieldEffect();
    }
}

