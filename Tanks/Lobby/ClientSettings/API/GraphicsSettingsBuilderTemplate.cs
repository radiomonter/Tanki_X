namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;

    [SerialVersionUID(0x8d3b144e5783f5eL)]
    public interface GraphicsSettingsBuilderTemplate : ConfigPathCollectionTemplate, Template
    {
        [AutoAdded]
        GraphicsSettingsBuilderComponent builder();
    }
}

