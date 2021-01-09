namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;

    [TemplatePart]
    public interface CartridgeCaseSettingTemplatePart : SettingsTemplate, Template
    {
        [AutoAdded]
        CartridgeCaseSettingComponent cartridgeCaseSettingComponent();
    }
}

