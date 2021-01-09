namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ed9249ec5L)]
    public class CaptchaImageEvent : Event
    {
        public byte[] CaptchaBytes { get; set; }
    }
}

