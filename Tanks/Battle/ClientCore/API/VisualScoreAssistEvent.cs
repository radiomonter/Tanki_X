namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15fe86378bcL)]
    public class VisualScoreAssistEvent : VisualScoreEvent
    {
        public string TargetUid { get; set; }

        public int Percent { get; set; }
    }
}

