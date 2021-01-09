namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x370152efc3217c32L)]
    public class PingResultEvent : Event
    {
        public long ServerTime { get; set; }

        public float Ping { get; set; }
    }
}

