namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x151af2f6e75L)]
    public class IncomingFriendAddedEvent : FriendAddedBaseEvent
    {
    }
}

