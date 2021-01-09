namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [TemplatePart, SerialVersionUID(0x8d47cfd7180ee14L)]
    public interface SpiderMineCameraShakerTemplatePart : SpiderEffectTemplate, EffectBaseTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ImpactCameraShakerConfigComponent impactCameraShakerConfig();
    }
}

