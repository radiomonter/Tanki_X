namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Lobby.ClientSettings.API;

    [TemplatePart]
    public interface TargetFocusSettingsTemplatePart : SettingsTemplate, Template
    {
        [AutoAdded]
        LaserSightSettingsComponent laserSightSettings();
        [AutoAdded]
        TargetFocusSettingsComponent targetFocusSettings();
    }
}

