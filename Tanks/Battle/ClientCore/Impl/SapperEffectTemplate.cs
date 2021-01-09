namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x169c3901fc4L)]
    public interface SapperEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        SapperEffectComponent sapperEffect();
    }
}

