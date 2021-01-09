namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x153aa07b1c6L)]
    public class IntroduceUserByEmailEvent : IntroduceUserEvent
    {
        public string Email { get; set; }
    }
}

