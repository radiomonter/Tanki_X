namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ef2a91b28L)]
    public class RequestRegisterUserEvent : Event
    {
        public string Uid { get; set; }

        public string EncryptedPasswordDigest { get; set; }

        public string Email { get; set; }

        public string HardwareFingerprint { get; set; }

        public bool Subscribed { get; set; }

        public bool Steam { get; set; }

        public bool QuickRegistration { get; set; }
    }
}

