﻿namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FriendAddedRemovedBaseEvent : Event
    {
        public long FriendId { get; set; }
    }
}
