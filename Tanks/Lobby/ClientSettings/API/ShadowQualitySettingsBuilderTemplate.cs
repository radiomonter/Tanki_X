namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;

    [SerialVersionUID(0x8d4a43a1997387aL)]
    public interface ShadowQualitySettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template
    {
        [AutoAdded]
        ShadowQualitySettingsBuilderComponent builder();
    }
}

