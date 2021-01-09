namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16021cc037eL)]
    public class StreakTerminationEvent : Event
    {
        public string VictimUid { get; set; }
    }
}

