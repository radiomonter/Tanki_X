namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x165f29e3e19L)]
    public class DisconectUserFromBlockedCountryEvent : Event
    {
    }
}

