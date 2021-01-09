namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15fe8632e23L)]
    public class VisualScoreKillEvent : VisualScoreEvent
    {
        public string TargetUid { get; set; }

        public int TargetRank { get; set; }
    }
}

