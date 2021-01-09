namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x151af30bcf2L)]
    public class OutgoingFriendAddedEvent : FriendAddedBaseEvent
    {
    }
}

