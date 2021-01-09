namespace tanks.modules.lobby.ClientUserProfile.Scripts.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ea4147760L)]
    public class UpdateUserLeaguePlaceClientEvent : Event
    {
        public long Place { get; set; }
    }
}

