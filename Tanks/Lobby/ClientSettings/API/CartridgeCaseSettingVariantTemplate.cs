namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientControls.API;

    [SerialVersionUID(0x1669c43d4eaL)]
    public interface CartridgeCaseSettingVariantTemplate : CarouselItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CartridgeCaseSettingVariantComponent variant();
    }
}

