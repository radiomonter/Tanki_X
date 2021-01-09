namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158dd0f33d2L)]
    public class NewItemNotificationComponent : Component
    {
        public Entity Item { get; set; }

        public int Amount { get; set; }
    }
}

