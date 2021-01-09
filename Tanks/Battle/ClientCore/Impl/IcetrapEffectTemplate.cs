namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4e4829dbc9777L)]
    public interface IcetrapEffectTemplate : MineEffectTemplate, EffectBaseTemplate, Template
    {
        [AutoAdded]
        IcetrapEffectComponent icetrapEffect();
    }
}

