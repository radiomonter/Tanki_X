namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;

    [TemplatePart]
    public interface PostProcessingQualityVariantTemplatePart : SettingsTemplate, Template
    {
        [AutoAdded]
        PostProcessingQualityVariantComponent postProcessingQualitySettings();
    }
}

