namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Lobby.ClientFriends.Impl;

    [Shared, SerialVersionUID(0x151a4c382f1L)]
    public class AcceptFriendEvent : FriendBaseEvent
    {
        public AcceptFriendEvent()
        {
        }

        public AcceptFriendEvent(Entity user) : base(user)
        {
        }
    }
}

