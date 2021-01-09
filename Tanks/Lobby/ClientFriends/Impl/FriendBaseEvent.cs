namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x151af34294eL)]
    public class FriendBaseEvent : Event
    {
        public FriendBaseEvent()
        {
        }

        public FriendBaseEvent(Entity user)
        {
            this.User = user;
        }

        public Entity User { get; set; }
    }
}

