namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientControls.API;

    [SerialVersionUID(0x8d4ac42af2b4eaeL)]
    public interface VegetationSettingsTemplate : CarouselItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        VegetationSettingsComponent variant();
    }
}

