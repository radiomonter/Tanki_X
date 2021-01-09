namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158aac35a52L)]
    public class BattlePingResultEvent : Event
    {
        public float ClientSendRealTime { get; set; }

        public float ClientReceiveRealTime { get; set; }
    }
}

