namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class QuestProgressAnimatorComponent : Component
    {
        public float ProgressPrevValue { get; set; }
    }
}

