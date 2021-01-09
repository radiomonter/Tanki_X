namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Lobby.ClientFriends.Impl;

    [Shared, SerialVersionUID(0x151a4c3cf04L)]
    public class RejectFriendEvent : FriendBaseEvent
    {
        public RejectFriendEvent()
        {
        }

        public RejectFriendEvent(Entity user) : base(user)
        {
        }
    }
}

