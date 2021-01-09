namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;

    [TemplatePart]
    public interface VSyncSettingTemplatePart : SettingsTemplate, Template
    {
        [AutoAdded]
        VSyncSettingComponent vSyncSettingComponent();
    }
}

