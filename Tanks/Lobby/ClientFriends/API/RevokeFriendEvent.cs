namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Lobby.ClientFriends.Impl;

    [Shared, SerialVersionUID(0x151aa77cb81L)]
    public class RevokeFriendEvent : FriendBaseEvent
    {
        public RevokeFriendEvent()
        {
        }

        public RevokeFriendEvent(Entity user) : base(user)
        {
        }
    }
}

