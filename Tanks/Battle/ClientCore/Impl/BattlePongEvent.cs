namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158aabb56b4L)]
    public class BattlePongEvent : Event
    {
        public float ClientSendRealTime { get; set; }
    }
}

