namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ShowNotificationEvent : Event
    {
        public ShowNotificationEvent(List<Entity> sortedNotifications)
        {
            this.CanShowNotification = true;
            this.SortedNotifications = sortedNotifications;
        }

        public bool CanShowNotification { get; set; }

        public List<Entity> SortedNotifications { get; private set; }
    }
}

