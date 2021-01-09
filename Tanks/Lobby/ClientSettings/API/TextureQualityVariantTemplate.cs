namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientControls.API;

    [SerialVersionUID(0x8d49e074c39c955L)]
    public interface TextureQualityVariantTemplate : CarouselItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TextureQualityVariantComponent variant();
    }
}

