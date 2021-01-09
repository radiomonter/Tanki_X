namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x152f8686963L)]
    public class EmailInvalidEvent : Event
    {
        public string Email { get; set; }
    }
}

