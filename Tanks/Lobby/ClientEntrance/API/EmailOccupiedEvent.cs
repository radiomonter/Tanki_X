namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d33162f3da2dacL)]
    public class EmailOccupiedEvent : Event
    {
        public string Email { get; set; }
    }
}

