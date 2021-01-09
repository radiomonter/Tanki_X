namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15406d5566eL)]
    public class RequestChangePasswordEvent : Event
    {
        public string PasswordDigest { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}

