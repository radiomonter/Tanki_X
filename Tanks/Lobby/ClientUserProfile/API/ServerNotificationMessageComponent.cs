namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ba97f77cdL)]
    public class ServerNotificationMessageComponent : Component
    {
        public string Message { get; set; }
    }
}

