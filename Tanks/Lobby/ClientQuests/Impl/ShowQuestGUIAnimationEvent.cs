namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShowQuestGUIAnimationEvent : Event
    {
        public float ProgressDelay { get; set; }
    }
}

