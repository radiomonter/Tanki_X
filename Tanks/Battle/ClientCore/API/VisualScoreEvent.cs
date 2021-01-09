namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class VisualScoreEvent : Event
    {
        public int Score { get; set; }
    }
}

