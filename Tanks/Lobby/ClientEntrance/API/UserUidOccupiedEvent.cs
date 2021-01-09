namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ecefb4d76L)]
    public class UserUidOccupiedEvent : Event
    {
        public string Uid { get; set; }
    }
}

