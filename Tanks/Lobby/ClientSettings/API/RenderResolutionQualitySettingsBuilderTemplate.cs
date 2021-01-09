namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;

    [SerialVersionUID(0x8d4c93108a48891L)]
    public interface RenderResolutionQualitySettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template
    {
        [AutoAdded]
        RenderResolutionQualitySettingsBuilderComponent builder();
    }
}

