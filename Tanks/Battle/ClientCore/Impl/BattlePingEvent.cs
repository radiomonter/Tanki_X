namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158aa4e85daL)]
    public class BattlePingEvent : Event
    {
        public float ClientSendRealTime { get; set; }
    }
}

