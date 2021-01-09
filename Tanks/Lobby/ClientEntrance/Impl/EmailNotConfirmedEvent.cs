namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x153c27234e2L)]
    public class EmailNotConfirmedEvent : Event
    {
        public string Email { get; set; }
    }
}

