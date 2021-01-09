namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d3af10d8250458L)]
    public interface CarouselItemTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CarouselItemTextComponent carouselItemText();
    }
}

