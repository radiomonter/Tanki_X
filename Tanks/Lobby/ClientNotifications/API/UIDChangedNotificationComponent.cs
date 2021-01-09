namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15799d18d6fL)]
    public class UIDChangedNotificationComponent : Component
    {
        public string OldUserUID { get; set; }
    }
}

