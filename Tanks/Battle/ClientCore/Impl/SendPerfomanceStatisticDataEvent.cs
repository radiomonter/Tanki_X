namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1566a17e5b0L)]
    public class SendPerfomanceStatisticDataEvent : Event
    {
        public SendPerfomanceStatisticDataEvent(PerformanceStatisticData data)
        {
            this.Data = data;
        }

        public PerformanceStatisticData Data { get; set; }
    }
}

