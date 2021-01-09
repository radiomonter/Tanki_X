namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15398ac99e8L)]
    public class UsersLoadedEvent : Event
    {
        public long RequestEntityId { get; set; }
    }
}

