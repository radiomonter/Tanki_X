namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class IntroduceUserEvent : Event
    {
        [ProtocolOptional]
        public string Captcha { get; set; }
    }
}

