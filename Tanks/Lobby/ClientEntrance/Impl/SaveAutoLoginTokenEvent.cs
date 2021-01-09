namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ed3aad3c9L)]
    public class SaveAutoLoginTokenEvent : Event
    {
        public string Uid { get; set; }

        public byte[] Token { get; set; }
    }
}

