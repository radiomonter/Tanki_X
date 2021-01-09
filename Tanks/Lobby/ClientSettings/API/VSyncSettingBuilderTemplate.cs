namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;

    [SerialVersionUID(0x167bb496898L)]
    public interface VSyncSettingBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template
    {
        [AutoAdded]
        VSyncSettingBuilderComponent builder();
    }
}

