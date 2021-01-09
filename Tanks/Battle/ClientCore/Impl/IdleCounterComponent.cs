namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x28ab22d81c52570eL)]
    public class IdleCounterComponent : Component
    {
        [ProtocolOptional]
        public Date? SkipBeginTime { get; set; }

        public long SkippedMillis { get; set; }
    }
}

