namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x159406d0f3eL)]
    public class SubscribeChangeEvent : Event
    {
        public bool Subscribed { get; set; }
    }
}

