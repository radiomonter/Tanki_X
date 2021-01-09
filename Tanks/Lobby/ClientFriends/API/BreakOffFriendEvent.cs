namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Lobby.ClientFriends.Impl;

    [Shared, SerialVersionUID(0x151aa86a04cL)]
    public class BreakOffFriendEvent : FriendBaseEvent
    {
        public BreakOffFriendEvent()
        {
        }

        public BreakOffFriendEvent(Entity user) : base(user)
        {
        }
    }
}

