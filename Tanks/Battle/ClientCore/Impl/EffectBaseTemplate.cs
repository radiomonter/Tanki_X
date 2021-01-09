namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x159fef60611L)]
    public interface EffectBaseTemplate : Template
    {
        EffectComponent effectComponent();
    }
}

