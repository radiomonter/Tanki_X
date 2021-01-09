namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FriendSentNotificationComponent : Component
    {
        public string Message { get; set; }
    }
}

