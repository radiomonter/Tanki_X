namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f2ec6866bL)]
    public class RoundReputationEarnedEvent : Event
    {
        public int Delta { get; set; }

        public bool UnfairMatching { get; set; }
    }
}

