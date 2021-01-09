namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.Impl;

    [TemplatePart, SerialVersionUID(0x8d44912326a43e6L)]
    public interface EffectHUDTemplate : EffectBaseTemplate, Template
    {
        [PersistentConfig("", false)]
        EffectIconComponent effectIcon();
    }
}

