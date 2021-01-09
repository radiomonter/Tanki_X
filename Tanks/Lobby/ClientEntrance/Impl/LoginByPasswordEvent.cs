namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14eb07d815bL)]
    public class LoginByPasswordEvent : Event
    {
        public string PasswordEncipher { get; set; }

        public bool RememberMe { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}

