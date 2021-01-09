namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a45a34522L)]
    public interface NormalizeTemperatureEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        NormalizeTemperatureEffectComponent normalizeTemperatureEffect();
    }
}

