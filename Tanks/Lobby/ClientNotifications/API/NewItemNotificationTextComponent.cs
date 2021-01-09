namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NewItemNotificationTextComponent : Component
    {
        public string HeaderText { get; set; }

        public string ItemText { get; set; }

        public string SingleItemText { get; set; }
    }
}

