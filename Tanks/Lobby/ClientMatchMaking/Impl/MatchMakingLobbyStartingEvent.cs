namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x15d3176e8a7L)]
    public class MatchMakingLobbyStartingEvent : Event
    {
    }
}

