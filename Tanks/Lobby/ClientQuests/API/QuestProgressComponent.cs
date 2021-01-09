namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x157ade76079L)]
    public class QuestProgressComponent : Component
    {
        public float PrevValue { get; set; }

        public float CurrentValue { get; set; }

        public float TargetValue { get; set; }

        public bool PrevComplete { get; set; }

        public bool CurrentComplete { get; set; }
    }
}

