namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientControls.API;

    [SerialVersionUID(0x8d3aefd0bcb81baL)]
    public interface QualitySettingsVariantTemplate : CarouselItemTemplate, Template
    {
        [AutoAdded]
        QualitySettingsVariantComponent variant();
    }
}

