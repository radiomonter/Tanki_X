namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Lobby.ClientFriends.Impl;

    [Shared, SerialVersionUID(0x151a4c1c018L)]
    public class RequestFriendEvent : FriendBaseEvent
    {
        public RequestFriendEvent()
        {
        }

        public RequestFriendEvent(Entity user) : base(user)
        {
        }
    }
}

