namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Lobby.ClientSettings.API;

    [TemplatePart]
    public interface GameCameraShakerSettingsTemplatePart : SettingsTemplate, Template
    {
        [AutoAdded]
        GameCameraShakerSettingsComponent gameCameraShakerSettings();
    }
}

