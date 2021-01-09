namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d286f204322dd1L)]
    public interface ScreenTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ScreenHeaderTextComponent screenHeaderText();
    }
}

