namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ed3fc622aL)]
    public class AutoLoginUserEvent : Event
    {
        public string Uid { get; set; }

        public byte[] EncryptedToken { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}

