namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;

    [SerialVersionUID(0x8d4a43a1382d233L)]
    public interface AntialiasingQualitySettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template
    {
        [AutoAdded]
        AntialiasingQualitySettingsBuilderComponent builder();
    }
}

