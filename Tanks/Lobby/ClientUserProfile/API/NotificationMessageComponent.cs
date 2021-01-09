namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NotificationMessageComponent : Component
    {
        public NotificationMessageComponent()
        {
        }

        public NotificationMessageComponent(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}

