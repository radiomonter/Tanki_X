namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14f217357bdL)]
    public class IntroduceUserByUidEvent : IntroduceUserEvent
    {
        public string Uid { get; set; }
    }
}

