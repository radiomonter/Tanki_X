namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d584798d5ed5c9L)]
    public class EventContainerNotificationComponent : Component
    {
        public long ContainerId { get; set; }
    }
}

