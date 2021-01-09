namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d450d781203a2cL)]
    public interface SonarEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        SonarEffectComponent sonarEffect();
    }
}

