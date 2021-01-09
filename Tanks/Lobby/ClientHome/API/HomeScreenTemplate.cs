namespace Tanks.Lobby.ClientHome.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d2a3e0ec1874f4L)]
    public interface HomeScreenTemplate : Template
    {
        [State, PersistentConfig("", false)]
        HomeScreenLocalizedStringsComponent homeScreenLocalizedStrings();
    }
}

