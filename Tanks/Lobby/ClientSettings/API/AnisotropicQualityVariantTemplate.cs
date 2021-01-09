namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientControls.API;

    [SerialVersionUID(0x8d49e0778f08c77L)]
    public interface AnisotropicQualityVariantTemplate : CarouselItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        AnisotropicQualityVariantComponent variant();
    }
}

