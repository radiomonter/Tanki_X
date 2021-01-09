namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;

    [SerialVersionUID(0x1669f584a84L)]
    public interface CartridgeCaseAmountSettingBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template
    {
        [AutoAdded]
        CartridgeCaseAmountSettingBuilderComponent builder();
    }
}

