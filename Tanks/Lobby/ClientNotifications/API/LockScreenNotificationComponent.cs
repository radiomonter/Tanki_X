namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LockScreenNotificationComponent : Component
    {
        public Entity ScreenEntity { get; set; }
    }
}

