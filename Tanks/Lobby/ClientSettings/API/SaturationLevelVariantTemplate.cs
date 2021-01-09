namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientControls.API;

    [SerialVersionUID(0x8d3b79e172fd67dL)]
    public interface SaturationLevelVariantTemplate : CarouselItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        SaturationLevelVariantComponent variant();
    }
}

