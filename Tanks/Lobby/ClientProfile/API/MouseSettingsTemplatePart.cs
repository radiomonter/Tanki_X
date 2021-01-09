namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Lobby.ClientSettings.API;

    [TemplatePart]
    public interface MouseSettingsTemplatePart : SettingsTemplate, Template
    {
        [AutoAdded]
        GameMouseSettingsComponent gameMouseSettings();
    }
}

