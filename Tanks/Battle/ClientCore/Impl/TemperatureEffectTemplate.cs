namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a212cdc10L)]
    public interface TemperatureEffectTemplate : EffectBaseTemplate, Template
    {
        [PersistentConfig("", false)]
        TemperatureEffectComponent temperatureEffect();
    }
}

