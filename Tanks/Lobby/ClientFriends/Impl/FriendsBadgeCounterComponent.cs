namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FriendsBadgeCounterComponent : Component
    {
        public int Counter { get; set; }
    }
}

