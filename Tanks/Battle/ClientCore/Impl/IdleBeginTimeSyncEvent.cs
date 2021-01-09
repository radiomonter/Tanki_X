namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x404e779bff9158e2L)]
    public class IdleBeginTimeSyncEvent : Event
    {
        public Date IdleBeginTime { get; set; }
    }
}

