namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x151af3035b9L)]
    public class IncomingFriendRemovedEvent : FriendRemovedBaseEvent
    {
    }
}

