namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0xf7ac66bec1e443bL)]
    public class PongEvent : Event
    {
        public float PongCommandClientRealTime { get; set; }

        public sbyte CommandId { get; set; }
    }
}

